using Members.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace Members.Pages
{
    public class MatchingPasswords : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var registrationForm = (RegistrationModel)validationContext.ObjectInstance;

            if (registrationForm != null && registrationForm.Password == registrationForm.ConfirmPassword)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? "Password does not match.");
            }
        }
    }
    public class RegistrationModel : PageModel
    {
        public IShape Shape { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MatchingPasswords]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string AccountType { get; set; }
        [Required]
        public string OIB { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string IsActive { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string County{ get; set; }
        public string Phone { get; set; }
        [Required]
        public string DateBirth { get; set; }
        public string Gender { get; set; }
        public string SkillsKnowledge { get; set; }

        [Required]
        public int EmployeeNumber { get; set; }
        [Required]
        public string Trade { get; set; }
        [Required]
        public string OrganisationType { get; set; }
        [Required]
        public float Turnover { get; set; }
        [Required]
        public int AssociatesNumber { get; set; }
        [Required]
        public string CompanyRepresentative { get; set; }
        [Required]
        public string Function { get; set; }


        private readonly MemberService _mService;

        public RegistrationModel(MemberService mService)
        {
            _mService = mService;
        }
        public List<ContentItem> CountyTerms { get; set; }
        public List<ContentItem> OrganisationTypeTerms { get; set; }

        public List<ContentItem> FunctionTerms { get; set; }

        public List<ContentItem> TradeTerms { get; set; }

        public List<ContentItem> ActiveMemberTerms { get; set; }
        public List<ContentItem> GenderTerms { get; set; }


        public async Task OnGetAsync()
        {

            CountyTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Županija");
            OrganisationTypeTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Vrsta organizacije/Pravni oblik");
            TradeTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Djelatnost");
            FunctionTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Funkcija");
            ActiveMemberTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Doprinos èlana");
            GenderTerms = (List<ContentItem>)await _mService.GetTaxonomyTerms("Spol");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await OnGetAsync ();

            //user=new User(); contentItem.userid = user.userid

            //ContentItem contentItem;
            //(contentItem, Shape) = await _mService.ModelToNew(ContentType.Member);

            //(contentItem, Shape) = await _mService.ModelToNew(ContentType.Company);

            Console.WriteLine(ModelState.IsValid);


            if (!ModelState.IsValid) {

                ModelState.AddModelError(string.Empty, "There are errors in the form.");


                //ModelState.AddModelError("ConfirmPassword", "Passwords don't match");

                return Page();
            }
            
            return RedirectToPage("RegistrationSuccess");
        }
    }
}
