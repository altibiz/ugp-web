using Members.Models;
using Members.Utils;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Members.Handlers
{
    public class PersonPartHandler:ContentPartHandler<PersonPart>
    {
        private readonly ISession _session;
        private readonly IStringLocalizer S;
        private readonly IContentDefinitionManager _cdm;

        public PersonPartHandler(
            ISession session,
            IStringLocalizer<PersonPartHandler> stringLocalizer, IContentDefinitionManager cdm)
        {
            _session = session;
            S = stringLocalizer;
            _cdm = cdm;
        }

        public override async Task ValidatingAsync(ValidateContentContext context, PersonPart part)
        {
            // Only validate the alias if it's not empty.
            if (string.IsNullOrWhiteSpace(part.Oib.Text))
            {
                return;
            }

            await foreach (var item in part.ValidateAsync(S, _session,_cdm.GetSettings<PersonPartSettings>(part)))
            {
                context.Fail(item);
            }
        }
    }
}
