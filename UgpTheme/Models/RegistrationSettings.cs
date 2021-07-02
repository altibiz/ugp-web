using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UgpTheme.Models
{
    public class RegistrationSettings
    {
        public UserRegistrationType UsersCanRegister { get; set; }
        public bool UsersMustValidateEmail { get; set; }
        public bool UseSiteTheme { get; set; }
        public bool NoPasswordForExternalUsers { get; set; }
        public bool NoUsernameForExternalUsers { get; set; }
        public bool NoEmailForExternalUsers { get; set; }
        public bool UseScriptToGenerateUsername { get; set; }
        public string GenerateUsernameScript { get; set; }
    }
}
