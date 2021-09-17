using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Users;

namespace Members.Pages
{
    public class LogOutModel : PageModel
    {
        private readonly SignInManager<IUser> _signInManager;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public LogOutModel(SignInManager<IUser> signInManager, INotifier notifier, IHtmlLocalizer<CreateCompanyModel> htmlLocalizer)
        {
            _signInManager = signInManager;
            _notifier = notifier;

            H = htmlLocalizer;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            _notifier.Success(H["SignOut Success"]);
            return Page();
        }
    }
}
