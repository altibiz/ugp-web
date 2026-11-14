using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Documents;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Locking.Distributed;
using OrchardCore.Modules;
using OrchardCore.BackgroundTasks;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;

namespace OrchardCore.Themes.UgpTheme.AutosyncRecipes
{
    /// <summary>
    /// Applies every recipe whose file name ends in <c>.autosync.recipe.json</c> when the tenant
    /// starts, if the file changed since it was last applied (SHA-256 of its contents).
    ///
    /// Autosync recipes may only contain "content" steps (pages, not schema). Content types,
    /// templates and features belong in data migrations (see Migrations/landing-page.recipe.json),
    /// which run before this. A recipe with any other step is refused.
    ///
    /// A recipe is skipped if one of its content items was changed
    /// since the last sync (e.g. edited in the admin), so admin edits aren't silently overwritten.
    /// Set <c>UgpTheme:AutosyncRecipes:OverwriteAdminEdits</c> to true to apply them anyway.
    /// Set <c>UgpTheme:AutosyncRecipes:Enabled</c> to false to turn the whole thing off.
    /// </summary>
    public class AutosyncRecipesTenantEvents : ModularTenantEvents
    {
        public const string FileSuffix = ".autosync.recipe.json";
        private const string ConfigSection = "UgpTheme:AutosyncRecipes";
        private const string LockKey = "UgpTheme.AutosyncRecipes";

        private static readonly JsonDocumentOptions _jsonOptions = new()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly ILogger _logger;

        public AutosyncRecipesTenantEvents(IShellHost shellHost, ShellSettings shellSettings, ILogger<AutosyncRecipesTenantEvents> logger)
        {
            _shellHost = shellHost;
            _shellSettings = shellSettings;
            _logger = logger;
        }

        public override Task ActivatedAsync()
        {
            // Must not run inside the activation (nor in a ShellScope deferred task, which still
            // runs inside it): activation holds the shell host's creation lock, and both our
            // IShellHost.GetScopeAsync calls and the recipe executor's per-step scopes wait on it,
            // which deadlocks the first request. A background task's GetScopeAsync simply waits
            // until activation completes. Don't flow the activation scope's AsyncLocal state.
            using (ExecutionContext.SuppressFlow())
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var scope = await _shellHost.GetScopeAsync(_shellSettings);

                        // Like Orchard's background tasks: some services (recipe environment, resource
                        // options) read IHttpContextAccessor.HttpContext. It's AsyncLocal, so the recipe
                        // executor's own per-step scopes see it too.
                        scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext =
                            scope.ShellContext.CreateHttpContext();

                        await scope.UsingAsync(s => SyncAsync(s.ServiceProvider));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Autosync recipes: could not start sync.");
                    }
                });
            }

            return Task.CompletedTask;
        }

        private static async Task SyncAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<AutosyncRecipesTenantEvents>>();
            var config = services.GetRequiredService<IShellConfiguration>().GetSection(ConfigSection);

            if (!config.GetValue("Enabled", true))
            {
                return;
            }

            var overwriteAdminEdits = config.GetValue("OverwriteAdminEdits", false);

            var shellHost = services.GetRequiredService<IShellHost>();
            var shellSettings = services.GetRequiredService<ShellSettings>();

            var (locker, locked) = await services.GetRequiredService<IDistributedLock>()
                .TryAcquireLockAsync(LockKey, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(10));

            if (!locked)
            {
                logger.LogInformation("Autosync recipes: another instance is already syncing, skipping.");
                return;
            }

            await using var _ = locker;

            try
            {
                var candidates = await LoadCandidatesAsync(services);
                if (candidates.Count == 0)
                {
                    return;
                }

                var state = await InScopeAsync(shellHost, shellSettings, sp =>
                    sp.GetRequiredService<IDocumentManager<AutosyncRecipesDocument>>().GetOrCreateImmutableAsync());

                foreach (var recipe in candidates.OrderBy(r => r.FileName, StringComparer.OrdinalIgnoreCase))
                {
                    if (recipe.NonContentSteps.Count > 0)
                    {
                        logger.LogError(
                            "Autosync recipes: refusing '{Recipe}' because it has non-content step(s) {Steps}. Autosync recipes may only contain 'content' steps; put schema changes in a data migration.",
                            recipe.FileName, string.Join(", ", recipe.NonContentSteps));
                        continue;
                    }

                    state.Recipes.TryGetValue(recipe.FileName, out var applied);

                    if (applied?.Hash == recipe.Hash)
                    {
                        continue;
                    }

                    if (applied != null && !overwriteAdminEdits)
                    {
                        var edited = await InScopeAsync(shellHost, shellSettings, sp =>
                            FindEditedItemsAsync(sp.GetRequiredService<IContentManager>(), applied));

                        if (edited.Count > 0)
                        {
                            logger.LogWarning(
                                "Autosync recipes: NOT applying '{Recipe}' because content item(s) {ContentItemIds} were changed since the last sync (probably edited in the admin). " +
                                "Move those edits into the recipe, or set {Setting} to true to overwrite them.",
                                recipe.FileName, string.Join(", ", edited), $"{ConfigSection}:OverwriteAdminEdits");
                            continue;
                        }
                    }

                    logger.LogInformation("Autosync recipes: applying '{Recipe}'.", recipe.FileName);

                    try
                    {
                        await InScopeAsync(shellHost, shellSettings, sp => ExecuteAsync(sp, recipe.Descriptor));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Autosync recipes: '{Recipe}' failed, it will be retried on next startup.", recipe.FileName);
                        continue;
                    }

                    // Fresh scope: the recipe steps ran (and committed) in their own scopes.
                    var contentItems = await InScopeAsync(shellHost, shellSettings, sp =>
                        SnapshotAsync(sp.GetRequiredService<IContentManager>(), recipe.ContentItemIds));

                    state = await InScopeAsync(shellHost, shellSettings, async sp =>
                    {
                        var manager = sp.GetRequiredService<IDocumentManager<AutosyncRecipesDocument>>();
                        var document = await manager.GetOrCreateMutableAsync();
                        document.Recipes[recipe.FileName] = new AppliedRecipe
                        {
                            Hash = recipe.Hash,
                            AppliedUtc = DateTime.UtcNow,
                            ContentItems = contentItems,
                        };
                        await manager.UpdateAsync(document);
                        return document;
                    });

                    logger.LogInformation("Autosync recipes: applied '{Recipe}'.", recipe.FileName);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Autosync recipes: sync failed.");
            }
        }

        private static async Task<List<Candidate>> LoadCandidatesAsync(IServiceProvider services)
        {
            var result = new List<Candidate>();

            foreach (var harvester in services.GetServices<IRecipeHarvester>())
            {
                foreach (var descriptor in await harvester.HarvestRecipesAsync())
                {
                    var file = descriptor.RecipeFileInfo;
                    if (descriptor.IsSetupRecipe || file == null ||
                        !file.Name.EndsWith(FileSuffix, StringComparison.OrdinalIgnoreCase) ||
                        result.Any(c => string.Equals(c.FileName, file.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    byte[] bytes;
                    await using (var stream = file.CreateReadStream())
                    using (var memory = new MemoryStream())
                    {
                        await stream.CopyToAsync(memory);
                        bytes = memory.ToArray();
                    }

                    var nonContentSteps = new List<string>();
                    var contentItemIds = new List<string>();

                    using (var json = JsonDocument.Parse(bytes, _jsonOptions))
                    {
                        if (json.RootElement.TryGetProperty("steps", out var steps))
                        {
                            foreach (var step in steps.EnumerateArray())
                            {
                                var name = step.TryGetProperty("name", out var n) ? n.GetString() : null;
                                if (!string.Equals(name, "content", StringComparison.OrdinalIgnoreCase))
                                {
                                    nonContentSteps.Add(name ?? "(unnamed)");
                                    continue;
                                }

                                if (step.TryGetProperty("data", out var data))
                                {
                                    foreach (var item in data.EnumerateArray())
                                    {
                                        if (item.TryGetProperty("ContentItemId", out var id) && id.GetString() is { Length: > 0 } s)
                                        {
                                            contentItemIds.Add(s);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    result.Add(new Candidate(
                        descriptor,
                        file.Name,
                        Convert.ToHexString(SHA256.HashData(bytes)),
                        nonContentSteps,
                        contentItemIds));
                }
            }

            return result;
        }

        private static async Task ExecuteAsync(IServiceProvider sp, RecipeDescriptor descriptor)
        {
            // Same environment the admin "Run recipe" action builds.
            var environment = new Dictionary<string, object>();
            foreach (var provider in sp.GetServices<IRecipeEnvironmentProvider>().OrderBy(p => p.Order))
            {
                await provider.PopulateEnvironmentAsync(environment);
            }

            var executionId = Guid.NewGuid().ToString("n");
            await sp.GetRequiredService<IRecipeExecutor>().ExecuteAsync(executionId, descriptor, environment, CancellationToken.None);
        }

        private static async Task<List<string>> FindEditedItemsAsync(IContentManager contentManager, AppliedRecipe applied)
        {
            var edited = new List<string>();

            foreach (var (id, snapshot) in applied.ContentItems)
            {
                var item = await contentManager.GetAsync(id, VersionOptions.Latest);
                if (item == null)
                {
                    // Deleted in the admin: re-creating it is what the recipe is for.
                    continue;
                }

                if (item.ContentItemVersionId != snapshot.ContentItemVersionId || TruncateToSeconds(item.ModifiedUtc) != TruncateToSeconds(snapshot.ModifiedUtc))
                {
                    edited.Add(id);
                }
            }

            return edited;
        }

        // The document store serializes DateTime without sub-second precision, content items keep it.
        private static DateTime? TruncateToSeconds(DateTime? value) =>
            value is { } v ? new DateTime(v.Ticks - v.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc) : null;

        private static async Task<Dictionary<string, AppliedContentItem>> SnapshotAsync(IContentManager contentManager, IEnumerable<string> ids)
        {
            var result = new Dictionary<string, AppliedContentItem>();

            foreach (var id in ids.Distinct())
            {
                var item = await contentManager.GetAsync(id, VersionOptions.Latest);
                if (item != null)
                {
                    result[id] = new AppliedContentItem
                    {
                        ContentItemVersionId = item.ContentItemVersionId,
                        ModifiedUtc = item.ModifiedUtc,
                    };
                }
            }

            return result;
        }

        private static async Task<T> InScopeAsync<T>(IShellHost shellHost, ShellSettings shellSettings, Func<IServiceProvider, Task<T>> action)
        {
            T result = default;
            var scope = await shellHost.GetScopeAsync(shellSettings);
            await scope.UsingAsync(async s => result = await action(s.ServiceProvider));
            return result;
        }

        private static Task InScopeAsync(IShellHost shellHost, ShellSettings shellSettings, Func<IServiceProvider, Task> action) =>
            InScopeAsync<object>(shellHost, shellSettings, async sp =>
            {
                await action(sp);
                return null;
            });

        private sealed record Candidate(
            RecipeDescriptor Descriptor,
            string FileName,
            string Hash,
            IReadOnlyList<string> NonContentSteps,
            IReadOnlyList<string> ContentItemIds);
    }
}
