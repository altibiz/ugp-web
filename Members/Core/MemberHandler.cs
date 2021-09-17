using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Members.Core
{
    class MemberHandler : ContentHandlerBase
    {
        public override Task ImportingAsync(ImportContentContext context)
        {
            if (context.ContentItem.ContentType == "Member")
            {
                FixMemberDate(context.ContentItem);
            }
            return Task.CompletedTask;
        }

        public static void FixMemberDate(ContentItem cItem)
        {
            if (cItem.Content.Member?.DateOfBirth?.Value is not JValue oldVal) return;
            if (oldVal?.Value is string strVal && DateTime.TryParseExact(Regex.Replace(strVal, "[A-Za-z]","").Replace("..","."),
                new[] { "d.M.yyyy", "d.M.yyyy.", "d.M.y.", "d.M.y", "d-M-yyyy","d-M-yy", "d-M-yyyy.", "ddMMyy", "ddMMyyyy","yyyy-M-d", "yyyy/M/d", "d/M/yyyy", "d/M/yyyy.", "d/M/yy", "d,M,yyyy,", "d,M,yyyy", "d M yyyy", "d.M yyyy", "d.M yyyy.","ddMM.yyyy","yyyy" },
                CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite,
                out DateTime dejt))
            {
                cItem.Content.Member.DateOfBirth.Value = dejt.ToString("s");
            }
            else if (!string.IsNullOrEmpty(oldVal?.Value?.ToString()))
            {
                cItem.Content.Member.DateOfBirthImport = oldVal?.Value;
                cItem.Content.Member.DateOfBirth.Value = null;
            }
        }
    }
}
