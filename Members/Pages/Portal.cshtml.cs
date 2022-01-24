﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Members.Core;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Alias.Models;
using OrchardCore.Menu.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using OrchardCore.ContentFields.Fields;
using OrchardCore.Admin;
using Microsoft.AspNetCore.Http;

namespace Members.Pages
{
    [Authorize]
    public class PortalModel : PageModel
    {
        private MemberService _mService;
        private INotifier _notifier;
        private IHtmlLocalizer<PortalModel> H;
        public ContentItem Member;
        public PortalModel(MemberService mService, INotifier notifier, IHtmlLocalizer<PortalModel> localizer)
        {
            _mService = mService;
            _notifier = notifier;
            H = localizer;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Member = await _mService.GetUserMember(true);

            if (Member == null)
            {
                return RedirectToPage("CreateMember");
            }
            if (!Member.Published)
            {
                await _notifier.InformationAsync(H["Molimo pričekajte da naši administratori potvrde prijavu"]);
            }
            return Page();
        }
    }
}

namespace Members.ContentHandlers
{
    public class LinkMenuItem : ContentPart
    {
        public TextField Icon { get; set; }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
    }
    public class UserMenuHandler : ContentHandlerBase
    {
        public UserMenuHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpCA = httpContextAccessor;
        }
        public IEnumerable<ContentItem> GetMenuCi()
        {
            return json.Select(x =>
            {
                var ci = new ContentItem
                {
                    ContentItemId = Guid.NewGuid().ToString(),
                    Published = true,
                    ContentType = "LinkMenuItem",
                    DisplayText = x.Name,
                };
                ci.Apply(new LinkMenuItemPart
                {
                    Url = x.Url
                });
                ci.Apply(new LinkMenuItem
                {
                    Icon = new TextField
                    {
                        Text = x.Text
                    }
                });
                return ci;
            });
        }

        public override Task LoadedAsync(LoadContentContext context)
        {
            if (context.ContentItem.ContentType == "Menu" && !
                AdminAttribute.IsApplied(_httpCA.HttpContext))
            {
                var alias = context.ContentItem.As<AliasPart>();
                if (alias != null)
                {
                    if (alias.Alias == "user-landing-page-menu")
                    {
                        var menulist = context.ContentItem.As<MenuItemsListPart>();
                        menulist.MenuItems = GetMenuCi().Concat(menulist.MenuItems).ToList();
                    }
                }
            }
            return Task.CompletedTask;
        }

        public static readonly MenuItem[] json = new[]{
            new MenuItem{
                Name= "Moji podaci",
                Url= "/Members/myprofile",
                Text= "far fa-address-card"
            },
            new MenuItem
            {
                Name="Moji Dokumenti",
                Url="/Members/MyDocuments",
                Text="far fa-file-alt"
            },
            new MenuItem
            {
                Name= "Doniraj",
                Url= "/Members/Donate",
                Text= "far fa-hand-point-up"
            },
            new MenuItem
            {

                Name= "Moje donacije",
                Url= "/Members/mydonations",
                Text= "fas fa-file-contract"
            },
            new MenuItem
            {
                Name= "Moja ponuda ",
                Url= "/Members/offerfor",
                Text= "fa fa-store"
            },
            new MenuItem
            {
                Name= "Ponude za članove",
                Url= "/Members/Offers",
                Text= "fas fa-percent"
            },
            new MenuItem
            {
                Name= "Događanja",
                Url= "/Members/events",
                Text= "far fa-calendar-alt"
            }
        };
        private IHttpContextAccessor _httpCA;
    }
}