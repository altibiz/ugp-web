using System;
using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Layout;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;

namespace UgpTheme.Drivers;

/// <summary>
/// When the content item set as the site homepage is displayed, switches it to the
/// Content-Homepage template and tells the layout to render the body full width.
/// The page's widgets stay admin-editable; only the presentation changes.
/// </summary>
public sealed class HomepageContentDisplayDriver : ContentDisplayDriver
{
    public const string LayoutFlag = "UgpHomepage";

    private readonly ISiteService _siteService;
    private readonly ILayoutAccessor _layoutAccessor;

    public HomepageContentDisplayDriver(ISiteService siteService, ILayoutAccessor layoutAccessor)
    {
        _siteService = siteService;
        _layoutAccessor = layoutAccessor;
    }

    public override async Task<IDisplayResult> DisplayAsync(ContentItem model, BuildDisplayContext context)
    {
        if (!string.Equals(context.DisplayType, "Detail", StringComparison.Ordinal))
        {
            return null;
        }

        var site = await _siteService.GetSiteSettingsAsync();
        if (site.HomeRoute == null
            || !site.HomeRoute.TryGetValue("contentItemId", out var homeId)
            || !string.Equals(homeId?.ToString(), model.ContentItemId, StringComparison.Ordinal))
        {
            return null;
        }

        context.Shape.Metadata.Alternates.Add("Content__Homepage");

        var layout = await _layoutAccessor.GetLayoutAsync();
        layout.Properties[LayoutFlag] = true;

        return null;
    }
}
