using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using YesSql;

namespace Members.Base
{
    public class FastImport : IRecipeStepHandler
    {
        public class ContentStepModel
        {
            public JsonArray Data { get; set; }
        }

        public Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!string.Equals(context.Name, "fastimport", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            var model = context.Step.ToObject<ContentStepModel>();
            var contentItems = model.Data.ToObject<ContentItem[]>();
            FastImportBackgroundTask.PendingImports.Enqueue(contentItems);

            return Task.CompletedTask;
        }
    }

    public class Importer
    {

        private const int ImportBatchSize = 500;
        private readonly ISession _session;

        public IEnumerable<IContentHandler> Handlers { get; }
        public IContentHandler[] ReversedHandlers { get; }

        private readonly ILogger<DefaultContentManager> _logger;

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
                    var context = new ImportContentContext(importingItem);
                    await Handlers.InvokeAsync((handler, context) => handler.ImportingAsync(context), context, _logger);
                    await _session.SaveAsync(importingItem);

                }
                await _session.SaveChangesAsync();
                skip += ImportBatchSize;
                _logger.LogDebug("Imported: " + skip);
                batchedContentItems = contentItems.Skip(skip).Take(ImportBatchSize);
            }
        }
    }

    [BackgroundTask(Schedule = "*/1 * * * *", Description = "Fast import background task.")]
    public class FastImportBackgroundTask : IBackgroundTask
    {
        public static readonly ConcurrentQueue<ContentItem[]> PendingImports = new();
        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            if (PendingImports.TryDequeue(out var toImport))
            {
                var contentManager = serviceProvider.GetRequiredService<Importer>();
                return contentManager.ImportAsync(toImport);
            }
            return Task.CompletedTask;
        }
    }
}
