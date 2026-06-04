using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using OrchardCore.Users;
using OrchardCore.Users.Models;
using OrchardCore.Users.ViewModels;

namespace Members.Users
{
    /// <summary>
    /// Replaces OrchardCore's RegisterUserFormDisplayDriver: the username is always
    /// the email address, so the form has no separate username field.
    /// </summary>
    public sealed class EmailRegisterUserFormDisplayDriver : DisplayDriver<RegisterUserForm>
    {
        private readonly UserManager<IUser> _userManager;
        private readonly IdentityOptions _identityOptions;
        private readonly IStringLocalizer S;

        public EmailRegisterUserFormDisplayDriver(
            UserManager<IUser> userManager,
            IOptions<IdentityOptions> identityOptions,
            IStringLocalizer<EmailRegisterUserFormDisplayDriver> stringLocalizer)
        {
            _userManager = userManager;
            _identityOptions = identityOptions.Value;
            S = stringLocalizer;
        }

        public override IDisplayResult Edit(RegisterUserForm model, BuildEditorContext context)
        {
            return Initialize<RegisterViewModel>("RegisterUserFormIdentifier", vm =>
            {
                vm.UserName = model.UserName;
                vm.Email = model.Email;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(RegisterUserForm model, UpdateEditorContext context)
        {
            var vm = new RegisterViewModel();

            await context.Updater.TryUpdateModelAsync(vm, Prefix);

            // The username is not posted by the form; it is always the email address,
            // so drop the [Required] validation error for it and copy the email over.
            vm.UserName = vm.Email;
            foreach (var key in context.Updater.ModelState.Keys
                .Where(k => k == nameof(vm.UserName) || k.EndsWith("." + nameof(vm.UserName)))
                .ToArray())
            {
                context.Updater.ModelState.Remove(key);
            }

            if (!string.IsNullOrEmpty(vm.UserName) && await _userManager.FindByNameAsync(vm.UserName) != null)
            {
                context.Updater.ModelState.AddModelError(Prefix, nameof(vm.Email), S["A user with the same email address already exists."]);
            }
            else if (_identityOptions.User.RequireUniqueEmail && !string.IsNullOrEmpty(vm.Email) && await _userManager.FindByEmailAsync(vm.Email) != null)
            {
                context.Updater.ModelState.AddModelError(Prefix, nameof(vm.Email), S["A user with the same email address already exists."]);
            }

            model.UserName = vm.UserName;
            model.Email = vm.Email;
            model.Password = vm.Password;

            return Edit(model, context);
        }
    }
}
