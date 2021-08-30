using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Members
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            // We want to add our menus to the "admin" menu only.
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            // Adding our menu items to the builder.
            // The builder represents the full admin menu tree.
            builder
                .Add(S["Članstvo"], "0", rootView => rootView
                   .Add(S["Fizičke osobe"], "5", childOne => childOne
                       .Action("List", "Admin", new { area = "OrchardCore.Contents",contentTypeId="Member" }))
                   .Add(S["Pravne osobe"], "6", childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents",contentTypeId="Company" }))
                   .Add(S["Donacije"], "7", childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Payment" }))
                   .Add(S["Ponude"], "8", childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Offer" }))
                 , new[] { "icon-class-fas", "icon-class-fa-users" });


            return Task.CompletedTask;
        }
    }
}
