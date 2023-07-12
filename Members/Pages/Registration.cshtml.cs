using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Members.Pages
{
    public class RegistrationModel : PageModel
    {

            [Required]
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
            public string SkillsKnowledge { get; set; }


            public string NameC { get; set; }
            public string AddressC { get; set; }
            public string CityC { get; set; }
            public string CountyC { get; set; }
            public int EmployeeNum { get; set; }
            public string Trade { get; set; }
            public string OrgType { get; set; }
            public float Turnover { get; set; }
            public int AssociatesNum { get; set; }
            public string CompanyRepresentative { get; set; }


            private readonly MemberService _mService;

            public RegistrationModel(MemberService mService)
            {
                _mService = mService;
            }
            public List<ContentItem> ZupanijaTerms { get; set; }
            public List<ContentItem> VrstaOrganizacijaTerms { get; set; }

            public List<ContentItem> FunkcijaTerms { get; set; }

            public List<ContentItem> DjelatnostTerms { get; set; }

            public List<ContentItem> Aktivan»lanTerms { get; set; }
            public List<ContentItem> SpolTerms { get; set; }


        public async Task OnGetAsync()
            {

                ZupanijaTerms = (List<ContentItem>) await _mService.GetTaxonomyTerms("éupanija");
                VrstaOrganizacijaTerms = (List<ContentItem>) await _mService.GetTaxonomyTerms("Vrsta organizacije/Pravni oblik");
                DjelatnostTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Djelatnost");
                FunkcijaTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Funkcija");
                Aktivan»lanTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Doprinos Ëlana");
                SpolTerms = (List<ContentItem>) await _mService.GetTaxonomyTerms("Spol");

        }


        }
    }

