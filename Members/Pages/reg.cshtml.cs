using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Members.Pages
{
    public class regModel : PageModel
    {
        public string Passwd { get; set; }
        public string Email { get; set; }
        public string ConfirmPasswd { get; set; }

        public string AccType { get; set; }

        public string OIB { get; set; }
        public string NameP { get; set; }
        public string Surname { get; set; }
        public string IsActive { get; set; }

        public string AddressP { get; set; }
        public string CityP { get; set; }
        public string CountyP { get; set; }
        public string Phone { get; set; }
        public string DateBirth { get; set; }
        public string Gender { get; set; }


        public string NameC { get; set; }
        public string AddressC { get; set; }
        public string CityC { get; set; }
        public string CountyC { get; set; }
        public int EmployeeNum { get; set; }
        public string Activity { get; set; }
        public string OrgType { get; set; }
        public float Turnover { get; set; }











    }
}
