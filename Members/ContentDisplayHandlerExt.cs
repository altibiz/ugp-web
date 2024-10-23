using Members.Base;
using Microsoft.AspNetCore.Http;
using OrchardCore.Admin;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Shapes;
using OrchardCore.Taxonomies.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Members
{
    internal class ContentDisplayHandlerExt : IContentDisplayHandler
    {
        private IHttpContextAccessor _httpCA;

        public ContentDisplayHandlerExt(IHttpContextAccessor httpCA)
        {
            _httpCA = httpCA;
        }

        public Task BuildDisplayAsync(ContentItem contentItem, BuildDisplayContext context)
        {
            return Task.CompletedTask;
        }

        public Task BuildEditorAsync(ContentItem contentItem, BuildEditorContext context)
        {
            HandleEditorShape(contentItem, context, context.Shape as Shape);
            return Task.CompletedTask;
        }

        private void HandleEditorShape(ContentItem contentItem, BuildEditorContext context, IShape theShape)
        {
            foreach (var item in theShape.Items.Where(x=>x is EditTextFieldViewModel or EditTagTaxonomyFieldViewModel or EditTaxonomyFieldViewModel or EditDateFieldViewModel))
            {
                (item as IShape).SetFieldSettingsExt(context.IsNew, AdminAttribute.IsApplied(_httpCA.HttpContext));
            }
            foreach (var val in theShape.Properties.Where(x => x.Key != "Parent").Select(x => x.Value).OfType<IShape>())
            {
                HandleEditorShape(contentItem, context, val);
            }
        }

        public Task UpdateEditorAsync(ContentItem contentItem, UpdateEditorContext context)
        {
            return Task.CompletedTask;
        }
    }
}
