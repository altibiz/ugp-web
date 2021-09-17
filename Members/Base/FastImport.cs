using Members.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Members.Base
{
    public class FastImport : IRecipeStepHandler
    {
        public class ContentStepModel
        {
            public JArray Data { get; set; }
        }

        public Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!string.Equals(context.Name, "fastimport", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            var model = context.Step.ToObject<ContentStepModel>();
            var contentItems = model.Data.ToObject<ContentItem[]>();

            // If the shell is activated there is no migration in progress.
            if (ShellScope.Context.IsActivated)
            {
                var contentManager = ShellScope.Services.GetRequiredService<Importer>();
                return contentManager.ImportAsync(contentItems);
            }

            // Otherwise, the import of content items is deferred after all migrations are completed,
            // this prevents e.g. a content handler to trigger a workflow before worflows migrations.
            ShellScope.AddDeferredTask(scope =>
            {
                var contentManager = scope.ServiceProvider.GetRequiredService<Importer>();
                return contentManager.ImportAsync(contentItems);
            });

            return Task.CompletedTask;
        }
    }

    public class Importer
    {

        private const int ImportBatchSize = 500;
        private ISession _session;

        public IEnumerable<IContentHandler> Handlers { get; }
        public IContentHandler[] ReversedHandlers { get; }

        private ILogger<DefaultContentManager> _logger;

        public Importer(ISession session, ILogger<DefaultContentManager> logger, IEnumerable<IContentHandler> handlers)
        {
            _session = session;
            Handlers = handlers;
            ReversedHandlers = handlers.Reverse().ToArray();
            _logger = logger;
        }


        public async Task ImportAsync(IEnumerable<ContentItem> contentItems)
        {
            var skip = 0;

            var importedVersionIds = new HashSet<string>();

            var batchedContentItems = contentItems.Take(ImportBatchSize);

            while (batchedContentItems.Any())
            {
                // Preload all the versions for this batch from the database.
                var versionIds = batchedContentItems
                     .Where(x => !String.IsNullOrEmpty(x.ContentItemVersionId))
                     .Select(x => x.ContentItemVersionId);

                var itemIds = batchedContentItems
                    .Where(x => !String.IsNullOrEmpty(x.ContentItemId))
                    .Select(x => x.ContentItemId);

                foreach (var importingItem in batchedContentItems)
                {
                    if (!string.IsNullOrEmpty(importingItem.ContentItemVersionId))
                    {
                        if (importedVersionIds.Contains(importingItem.ContentItemVersionId))
                        {
                            continue;
                        }

                        importedVersionIds.Add(importingItem.ContentItemVersionId);
                    }
                    MemberHandler.FixMemberDate(importingItem);
                    var context = new ImportContentContext(importingItem);
                    _session.Save(importingItem);

                }

                await _session.FlushAsync();
                skip += ImportBatchSize;
                _logger.LogDebug("Imported: " + skip);
                batchedContentItems = contentItems.Skip(skip).Take(ImportBatchSize);
            }
        }
    }
}
