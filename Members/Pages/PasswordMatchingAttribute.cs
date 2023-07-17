using System.ComponentModel.DataAnnotations;


namespace Members.Pages { 
    public class PasswordMatchingAttribute : ValidationAttribute
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
    }

