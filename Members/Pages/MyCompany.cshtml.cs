using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;
using OrchardCore.Contents;
using OrchardCore;

namespace Members.Pages
{
    public class MyCompanyModel : PageModel
    {
        private const string memberType = "Member";
        private const string companyType = "Company";

        private readonly ISession _session;
        private readonly IOrchardHelper _orchardHelper;
        private readonly IContentManager _contentManager;

        //uneditable
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public string Oib { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string AuthorizedRepresentative { get; set; }


        [BindProperty]
        public string TurnoverIn2019 { get; set; }
        [BindProperty]
        public string Activity { get; set; }
        [BindProperty]
        public string EmployeeNumber { get; set; }
        [BindProperty]
        public string PermanentAssociates { get; set; }
        [BindProperty]
        public string Function { get; set; }
        [BindProperty]
        public string MemberActivity { get; set; }
        [BindProperty]
        public string Contribution { get; set; }


        [BindProperty]
        public dynamic ContentItem { get; set; }


        public MyCompanyModel(ISession session, IOrchardHelper orchardHelper, IContentManager contentManager)
        {
            _session = session;
            _orchardHelper = orchardHelper;
            _contentManager = contentManager;
        }


        public async Task OnGetAsync(string companyId)
        {
            var company = await _contentManager.GetAsync(companyId);


            ContentItem = company;

            Name = company.Content.Company.Name.Text;
            OrganisationType = company.Content.Company.OrganisationType.TagNames.ToString();
            Oib = company.Content.Company.Oib.Text;
            City = company.Content.Company.City.Text;
            Address = company.Content.Company.Address.Text;
            AuthorizedRepresentative = company.Content.Company.AuthorizedRepresentative.Text;

            TurnoverIn2019 = company.Content.Company.TurnoverIn2019.Text;
            Activity = company.Content.Company.Activity.Text;
            EmployeeNumber = company.Content.Company.EmployeeNumber.Value;
            PermanentAssociates = company.Content.Company.PermanentAssociates.Value;
            Function = company.Content.Company.Function.TagNames.ToString();
          //  MemberActivity = company.Content.Company.MemberActivity.Text;


        }
    }
}
