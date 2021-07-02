using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgpTheme.ViewModels
{
    public class LoginViewModel : IValidatableObject
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                yield return new ValidationResult("Username is required.", new[] { "UserName" });
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Password is required.", new[] { "Password" });
            }
        }
    }
}
