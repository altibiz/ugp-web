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

        public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            // We want to add our menus to the "admin" menu only.
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return ValueTask.CompletedTask;
            }

            // Adding our menu items to the builder.
            // The builder represents the full admin menu tree.
            builder
                .Add(S["Članstvo"], "0", rootView => rootView
                   .Add(S["Fizičke osobe"], "5", childOne => childOne
                       .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Member" }))
                   .Add(S["Pravne osobe"], "6", childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Company" }))
                   .Add(S["Ponude"], "8", childTwo => childTwo
                       .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Offer" }))
                 , ["icon-class-fas", "icon-class-fa-users"])
                .Add(S["Financije"], "0", rootView => rootView
                    .Add(S["Uplate"], "7", childTwo => childTwo
                        .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Payment", q = "payout:false" }))
                     .Add(S["Isplate"], "7", childTwo => childTwo
                        .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Payment", q = "payout:true" }))
                    .Add(S["Izvodi"], "9", childTwo => childTwo
                        .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "BankStatement" }))
                    .Add(S["Uplatnice"], "10", childTwo => childTwo
                        .Action("List", "Admin", new { area = "OrchardCore.Contents", contentTypeId = "Pledge" }))
                 , ["icon-class-fas", "icon-class-fa-coins"])
                .Add(S["Configuration"], "0", rootView => rootView
                .Add(S["Import/Export"], S["Import/Export"].PrefixPosition(), rootView => rootView
                .Add(S["Members Export"], "1", childTwo => childTwo
                    .Action("Index", "MembersExport", new { area = "Members"})))
                , ["icon-class-fas", "icon-class-fa-coins"]);

            return ValueTask.CompletedTask;
        }
    }
}
