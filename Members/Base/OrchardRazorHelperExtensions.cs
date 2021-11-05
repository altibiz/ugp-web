using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Razor;
using System.Threading.Tasks;

namespace Members.Base
{
    public static class OrchardRazorHelperExtensions
    {
        public static async Task<IHtmlContent> EditorAsync(this IOrchardDisplayHelper orchardDisplayHelper, ContentItem content, string displayType = "", string groupId = "", IUpdateModel updater = null)
        {
            var displayManager = orchardDisplayHelper.HttpContext.RequestServices.GetRequiredService<IContentItemDisplayManager>();
            var shape = await displayManager.BuildEditorAsync(content, updater,true,groupId);
            return await orchardDisplayHelper.DisplayHelper.ShapeExecuteAsync(shape);
        }

        public static async Task<IHtmlContent> EditorAsync(this IOrchardDisplayHelper orchardDisplayHelper, string contentType, string displayType = "", string groupId = "", IUpdateModel updater = null)
        {
            var contentManager = orchardDisplayHelper.HttpContext.RequestServices.GetRequiredService<IContentManager>();
            ContentItem content = await contentManager.NewAsync(contentType);
            return await orchardDisplayHelper.EditorAsync(content, displayType, groupId, updater);
        }
    }
}