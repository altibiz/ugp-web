using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Members.Core;
using Members.Persons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore;
using OrchardCore.ContentManagement;
using UgpTheme.ViewModels;

namespace Members.Pages
{
    public class MyProfileModel : PageModel
    {
        private const string memberType = "Member";
        private const string companyType = "Company";
        private readonly MemberService _mService;
        private readonly IOrchardHelper _oHelper;

        [BindProperty]
        public dynamic Activity { get; set; }
        [BindProperty]
        public string Contribution { get; set; }

        //Member
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }

        [BindProperty]
        public List<dynamic> CompanyContentItems { get; set; }

        public ContentItem Member { get; set; }

        public DropDownViewModel DropDownViewModel { get; set; }

        public MyProfileModel(MemberService service,IOrchardHelper orchardHelper)
        {

            _mService = service;
            _oHelper = orchardHelper;
        }

        public async Task OnGetAsync()
        {

            DropDownViewModel ddm = new DropDownViewModel();
            ddm.TaxonomyName = "aktivnost-clana";

            Member = await _mService.GetUserMember();
            if (Member == null) RedirectToPage("CreateMember");

            var personPart = Member.As<PersonPart>();
            var memberPart = Member.As<Member>();
            
            // Prepare the content items Summary Admin shape
            var companyContentItem = new List<dynamic>();
            foreach (var contentItem in await _oHelper.QueryListItemsAsync(Member.ContentItemId))
            {
                companyContentItem.Add(contentItem);
            }
            CompanyContentItems = companyContentItem;

            Name = personPart.Name.Text;

            Surname = personPart.Surname.Text;
            DateOfBirth =memberPart.DateOfBirth.Value;
            Address = personPart.Address.Text;
            City = personPart.City.Text;
            County = personPart.County.TermContentItemIds.FirstOrDefault();
            Email = personPart.Email?.Text;
            Phone = personPart.Phone.Text;
            Sex = memberPart.Sex.TermContentItemIds.FirstOrDefault();
            Activity = memberPart.Activity;
            Contribution = memberPart.Skills.Text;

        }
    }
}
