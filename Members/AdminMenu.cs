using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            // Adding our menu items to the builder.
            // The builder represents the full admin menu tree.
            builder
                .Add(S["Članstvo"], "0", rootView => rootView
                   .Add(S["Fizičke osobe"], S["Child One"].PrefixPosition(), childOne => childOne
                       .Action("List", "Admin", new { area = "OrchardCore.Contents",contentTypeId="Member" }))
                   .Add(S["Pravne osobe"], S["Child Two"].PrefixPosition(), childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents",contentTypeId="Company" })),new[] { "icon-class-fas","icon-class-fa-users" });

            return Task.CompletedTask;
        }
    }
}
