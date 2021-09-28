using Members.Core;
using Microsoft.AspNetCore.Http;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Lists.Models;

namespace Members.Base
{
    public class CpVm
    {
        public string ListContentItemId { get; set; }
        public string ParentName { get; set; }
    }

    public class ContainedPartDisplayDriver : ContentDisplayDriver
    {
        private IHttpContextAccessor _httpCA;
        public ContainedPartDisplayDriver(IHttpContextAccessor httpContextAccessor, MemberService memberService)
        {
            _httpCA = httpContextAccessor;
        }

        public override IDisplayResult Edit(ContentItem item)
        {
            if (!AdminAttribute.IsApplied(_httpCA.HttpContext)) return null;
            
            var part = item.As<ContainedPart>();
            if (part == null) return null;

            return Initialize<CpVm>("ContainedPart_Nav", m => { m.ListContentItemId = part.ListContentItemId; })
            .Location("Content");
        }
    }
}