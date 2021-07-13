using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Members.Pages
{
    public class MyProfileModel : PageModel
    {
        private const string memberType = "Member";
        private const string companyType = "Company";


        [BindProperty]
        public string TurnoverIn2019 { get; set; }
        [BindProperty]
        public string CompanyActivity { get; set; }
        [BindProperty]
        public int EmployeeNumber { get; set; }
        [BindProperty]
        public int PermanentAssociates { get; set; }
        [BindProperty]
        public string Function { get; set; }
        [BindProperty]
        public string MemberActivity { get; set; }
        [BindProperty]
        public string Contribution { get; set; }

        //company
        public string CompanyName { get; set; }
        public string OrganisationType { get; set; }
        public string CompanyOib { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAuthorizedRepresentative { get; set; }

        //Member
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }
        public string MemberBirthDate { get; set; }
        public string MemberAddress { get; set; }
        public string MemberCity { get; set; }
        public string MemberCounty { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhone { get; set; }
        public string MemberSex { get; set; }



        public MyProfileModel()
        {

        }


        public async Task OnGetAsync()
        {
        }
    }
}
