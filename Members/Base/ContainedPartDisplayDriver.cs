﻿using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Lists.Models;
using System.Threading.Tasks;

namespace Members.Base
{
    public class CpVm
    {
        public string ListContentItemId { get; set; }
        public string ParentName { get; set; }
    }

    public class ContainedPartDisplayDriver : ContentDisplayDriver
    {
        private readonly IHttpContextAccessor _httpCA;

        private readonly IContentManager _contentManager;

        public ContentItem MemberContentItem { get; set; }
        public ContainedPartDisplayDriver(IHttpContextAccessor httpContextAccessor, IContentManager cman)
        {
            _httpCA = httpContextAccessor;
            _contentManager = cman;
        }

        public override async Task<IDisplayResult> EditAsync(ContentItem model, BuildEditorContext context)
        {
            if (!AdminAttribute.IsApplied(_httpCA.HttpContext)) return null;

            var part = model.AsInit<ContainedPart>();
            if (part == null) return null;

            MemberContentItem = await _contentManager.GetAsync(part.ListContentItemId);

            return Initialize<CpVm>("ContainedPart_Nav", m => { m.ListContentItemId = part.ListContentItemId; m.ParentName = MemberContentItem.DisplayText; }).Location("Content");

        }
    }
}
