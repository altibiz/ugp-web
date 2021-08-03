using Members.Indexes;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YesSql;

namespace Members.Models
{
    public static class PersonPartExtensions
    {
        public static async IAsyncEnumerable<ValidationResult> ValidateAsync(this PersonPart part, IStringLocalizer S, ISession session, 
            PersonPartSettings personPartSettings)
        {
            if (!string.IsNullOrWhiteSpace(part.Oib.Text))
            {
                var oib = part.Oib.Text;
                if (oib.Length!=11)
                {
                    yield return new ValidationResult(S["Your ID must be 11 numbers"]);
                }

                if (!await IsPersonUniqueAsync(part, session, oib))
                {
                    yield return new ValidationResult(S["Your ID is already in use."]);
                }
            }
            if (personPartSettings?.PersonType != PersonType.Legal && string.IsNullOrWhiteSpace(part.Surname.Text))
            {
                yield return new ValidationResult(S["Surname is required"]);
            }

        }

        public static async Task<bool> IsPersonUniqueAsync(this PersonPart context, ISession session, string oib)
        {
            return (await session.QueryIndex<PersonPartIndex>(o => o.Oib == oib && o.ContentItemId != context.ContentItem.ContentItemId).CountAsync()) == 0;
        }

    }
}
