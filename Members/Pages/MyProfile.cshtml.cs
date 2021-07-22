using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Settings;
using OrchardCore.Users.Services;
using UgpTheme.ViewModels;
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
        public dynamic Activity { get; set; }
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

        public DropDownViewModel DropDownViewModel { get; set; }

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

            DropDownViewModel ddm = new DropDownViewModel();
            ddm.TaxonomyName = "aktivnost-clana";

            var user = await _userService.GetAuthenticatedUserAsync(User) as OrchardCore.Users.Models.User;

            var query = _session.Query<ContentItem, UserPickerFieldIndex>();
            query = query.With<UserPickerFieldIndex>(x => x.ContentType == memberType && x.Published && x.SelectedUserId == user.UserId);


            var member = await query.ListAsync();

            var companyQuery = _session.Query<ContentItem, ContentItemIndex>();
            companyQuery = companyQuery.With<ContentItemIndex>(x => x.ContentType == companyType && x.Published);
            companyQuery = companyQuery.OrderByDescending(x => x.PublishedUtc);

            var conmpanyContentItems = await companyQuery.ListAsync();

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
            County = MemberContentItem.Content.Member.County.TagNames[0];
            //Email = MemberContentItem.Content.Member.Email.Text;
            Phone = MemberContentItem.Content.Member.Phone.Text;
            Sex = MemberContentItem.Content.Member.Sex.TagNames[0];


            Activity = MemberContentItem.Content.Member.Activity;
            Contribution = MemberContentItem.Content.Member.Skills.Text;

        }
    }
}
