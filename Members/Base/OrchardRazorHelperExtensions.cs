using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Razor;
using System;
using System.Threading.Tasks;

namespace Members.Base
{
    public static class OrchardRazorHelperExtensions
    {
        public static async Task<IHtmlContent> EditorAsync(this IOrchardDisplayHelper orchardDisplayHelper, ContentItem content, string groupId = "", IUpdateModel updater = null)
        {
            var displayManager = orchardDisplayHelper.HttpContext.RequestServices.GetRequiredService<IContentItemDisplayManager>();
            if (updater == null)
            {
                updater = orchardDisplayHelper.HttpContext.RequestServices.GetRequiredService<IUpdateModelAccessor>().ModelUpdater;
            }
            var shape = await displayManager.BuildEditorAsync(content, updater, true, groupId);
            return await orchardDisplayHelper.DisplayHelper.ShapeExecuteAsync(shape);
        }

        public static async Task<IHtmlContent> EditorAsync(this IOrchardDisplayHelper orchardDisplayHelper, string contentType, string groupId = "", IUpdateModel updater = null, Action<ContentItem> onContentInit = null)
        {
            var contentManager = orchardDisplayHelper.HttpContext.RequestServices.GetRequiredService<IContentManager>();
            ContentItem content = await contentManager.NewAsync(contentType);
            onContentInit?.Invoke(content);
            return await orchardDisplayHelper.EditorAsync(content, groupId, updater);
        }
    }
}