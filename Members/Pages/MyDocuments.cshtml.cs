using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using Members.Core;
using System.Collections.Generic;

namespace Members.Pages
{
    public class MyDocumentsModel : PageModel
    {
        private readonly IHtmlLocalizer H;
        private readonly MemberService _memberService;
        public Dictionary<string, List<(string, string)>> DocLinks { get; } = new();
        public MyDocumentsModel(MemberService mService, IHtmlLocalizer<MyDocumentsModel> htmlLocalizer, INotifier notifier)
        {
            H = htmlLocalizer;
            _memberService = mService;
        }

        public async Task OnGetAsync(string companyId)
        {
            var companies = await _memberService.GetUserCompanies();
            var member = await _memberService.GetUserMember();
            AddLink(H["Membership"].Value, member.DisplayText, member.ContentItemId);
            foreach(var cmp in companies)
            {
                AddLink(H["Membership"].Value, cmp.DisplayText, cmp.ContentItemId);
            }
        }

        private void AddLink(string group, string name, string id)
        {
            if (!DocLinks.TryGetValue(group, out var links)) DocLinks[group] = links = new List<(string, string)>();
            links.Add((name, id));
        }
    }
}