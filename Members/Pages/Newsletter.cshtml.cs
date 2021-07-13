using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using OrchardCore.Taxonomies;
using OrchardCore.Users.Services;
using YesSql;

namespace Members.Pages
{
    public class NewsletterModel : PageModel
    {
        private const string memberType = "Member";
        private const string txnType = "Taxonomy";
        private const string companyType = "Company";

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IHtmlLocalizer H;
        private readonly dynamic New;
        private readonly INotifier _notifier;
        private readonly ISession _session;
        private readonly ISiteService _siteService;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IUserService _userService;

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Surname { get; set; }

        [BindProperty]
        public string Company { get; set; }

        [BindProperty]
        public string Country { get; set; }

        [BindProperty]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        public string Activity { get; set; }

        [BindProperty]
        public string Sex { get; set; }

        [BindProperty]
        public string UserType { get; set; }
        [BindProperty]
        public string CompanyType { get; set; }

        [BindProperty]
        public string MobileNumber { get; set; }

        [BindProperty]
        public string Subscribed { get; set; }

        [BindProperty]
        public string County { get; set; }

        [BindProperty]
        public string Place{ get; set; }

        public int[] TermContentItemIds { get; set; }


        [BindProperty]
        public List<dynamic> ContentItems { get; set; }
        [BindProperty]
        public dynamic Pager { get; set; }


        public NewsletterModel(IUserService userService, IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<MembersModel> htmlLocalizer, INotifier notifier, ISession session, IShapeFactory shapeFactory, ISiteService siteService, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _notifier = notifier;
            _session = session;
            _siteService = siteService;
            _updateModelAccessor = updateModelAccessor;
            _userService = userService;
            H = htmlLocalizer;
            New = shapeFactory;
        }

        public async Task OnGetAsync(PagerParameters pagerParameters)
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);


           // var user = await _userService.GetAuthenticatedUserAsync(User);

            var query = _session.Query<ContentItem, ContentItemIndex>();
            query = query.With<ContentItemIndex>(x => x.ContentType == memberType);
            query = query.With<ContentItemIndex>(x => x.Published);
            query = query.OrderByDescending(x => x.PublishedUtc);


            var maxPagedCount = siteSettings.MaxPagedCount;
            if (maxPagedCount > 0 && pager.PageSize > maxPagedCount)
                pager.PageSize = maxPagedCount;

            var routeData = new RouteData();
            var pagerShape = (await New.Pager(pager)).TotalItemCount(maxPagedCount > 0 ? maxPagedCount : await query.CountAsync()).RouteData(routeData);

            var pageOfContentItems = await query.ListAsync();
            IEnumerable<ContentItem> model = await query.ListAsync();

            // Prepare the content items Summary Admin shape
            var contentItemSummaries = new List<dynamic>();
            foreach (var contentItem in pageOfContentItems)
            {
                contentItemSummaries.Add(await _contentItemDisplayManager.BuildDisplayAsync(contentItem, _updateModelAccessor.ModelUpdater, "Summary"));
            }

            ContentItems = contentItemSummaries;
            Pager = pagerShape;
            //Options = model.Options
        }
    }
}
