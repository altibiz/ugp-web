using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Settings;
using OrchardCore.Users.Services;
using YesSql;

namespace Members.Pages
{
    public class MyProfileModel : PageModel
    {
        private const string memberType = "Member";
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
        public string Activity { get; set; }
        [BindProperty]
        public string Contribution { get; set; }

        //Member
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }

        [BindProperty]
        public List<dynamic> CompanyContentItems { get; set; }

        public dynamic MemberContentItem { get; set; }

        public MyProfileModel(IUserService userService, IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, IContentItemDisplayManager contentItemDisplayManager, IHtmlLocalizer<MembersModel> htmlLocalizer, INotifier notifier, ISession session, IShapeFactory shapeFactory, ISiteService siteService, IUpdateModelAccessor updateModelAccessor)
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

        public async Task OnGetAsync()
        {
            var user = await _userService.GetAuthenticatedUserAsync(User);

            var query = _session.Query<ContentItem, ContentItemIndex>();
            query = query.With<ContentItemIndex>(x => x.ContentType == memberType);
            query = query.With<ContentItemIndex>(x => x.Published);
            query = query.OrderByDescending(x => x.PublishedUtc);



            var companyQuery = _session.Query<ContentItem, ContentItemIndex>();
            companyQuery = companyQuery.With<ContentItemIndex>(x => x.ContentType == companyType);
            companyQuery = companyQuery.With<ContentItemIndex>(x => x.Published);
            companyQuery = companyQuery.OrderByDescending(x => x.PublishedUtc);

            var conmpanyContentItems = await companyQuery.ListAsync();

            var member = await query.ListAsync();

            MemberContentItem = member.FirstOrDefault();

            
            // Prepare the content items Summary Admin shape
            var companyContentItem = new List<dynamic>();
            foreach (var contentItem in conmpanyContentItems)
            {
                companyContentItem.Add(contentItem);
            }
            CompanyContentItems = companyContentItem;

            Name = MemberContentItem.Content.Member.Name.Text;

            Name = MemberContentItem.Content.Member.Name.Text;
            Surname = MemberContentItem.Content.Member.Surname.Text;
            DateOfBirth = MemberContentItem.Content.Member.DateOfBirth.Text;
            Address = MemberContentItem.Content.Member.Address.Text;
            City = MemberContentItem.Content.Member.City.Text;
            County = MemberContentItem.Content.Member.County.TagNames.ToString();
            //Email = MemberContentItem.Content.Member.Email.Text;
            Phone = MemberContentItem.Content.Member.Phone.Text;
            Sex = MemberContentItem.Content.Member.Sex.TagNames;


            Activity = MemberContentItem.Content.Member.Activity.Text;
            Contribution = MemberContentItem.Content.Member.Skills.Text;

        }
    }
}
