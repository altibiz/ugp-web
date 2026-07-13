using OrchardCore.Users.ViewModels;

namespace Members.Users
{
    /// <summary>
    /// Extends OrchardCore's <see cref="RegisterViewModel"/> with an explicit
    /// privacy-policy consent flag captured at registration (GDPR Art. 6/7).
    /// </summary>
    public class EmailRegisterViewModel : RegisterViewModel
    {
        public bool AcceptPrivacyPolicy { get; set; }
    }
}
