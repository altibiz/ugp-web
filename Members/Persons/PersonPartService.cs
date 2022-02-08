using Members.PartFieldSettings;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YesSql;
using ISession = YesSql.ISession;

namespace Members.Persons
{
    public class PersonPartService : IPartService<PersonPart>
    {
        private readonly ISession session;

        public IStringLocalizer<PersonPart> S { get; }

        private readonly IContentDefinitionManager _cdm;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public PersonPartService(IStringLocalizer<PersonPart> S, ISession session, IContentDefinitionManager cdm,
            IHttpContextAccessor httpContextAccessor, IUserService service
            )
        {
            this.session = session;
            this.S = S;
            _cdm = cdm;
            _httpContextAccessor = httpContextAccessor;
            _userService = service;
        }

        public async IAsyncEnumerable<ValidationResult> ValidateAsync(PersonPart part)
        {
            if (part.ContentItem.ContentItemId.StartsWith("nat_"))
            {
                yield break;
            }
            var personPartSettings = _cdm.GetSettings<PersonPartSettings>(part);
            if (!string.IsNullOrWhiteSpace(part.Oib?.Text))
            {
                var oib = part.Oib.Text;
                if (oib.Length != 11)
                {
                    yield return new ValidationResult(S["Your ID must be 11 numbers."]);
                }

                if (!await IsPersonUniqueAsync(part, oib))
                {
                    yield return new ValidationResult(S["Your ID is already in use."]);
                }
            }
            if (personPartSettings?.Type != PersonType.Legal && string.IsNullOrWhiteSpace(part.Surname.Text))
            {
                yield return new ValidationResult(S["Surname is required."]);
            }

        }

        private async Task<bool> IsPersonUniqueAsync(PersonPart part, string oib)
        {
            var typeDef = _cdm.GetSettings<PersonPartSettings>(part);
            var personType = typeDef.Type?.ToString();
            return (await session.QueryIndex<PersonPartIndex>(o => o.Oib == oib && o.ContentItemId != part.ContentItem.ContentItemId && o.PersonType == personType).CountAsync()) == 0;
        }

        public async Task<IEnumerable<PersonPartIndex>> GetByOibAsync(string oib)
        {
            return await session.QueryIndex<PersonPartIndex>(o => o.Oib == oib).ListAsync();
        }

        private async Task<User> GetCurrentUser()
        {
            return await _userService.GetAuthenticatedUserAsync(_httpContextAccessor.HttpContext?.User) as User;
        }

        public async Task InitializingAsync(PersonPart part)
        {
            var user = await GetCurrentUser();
            if (user == null) return;
            part.Email = new TextField { Text = user.Email };
        }

        public Task PublishedAsync(PersonPart instance, PublishContentContext context)
        {
            return Task.CompletedTask;
        }

        public System.Action<PersonPart> GetEditModel(PersonPart part, BuildPartEditorContext context)
        {
            return null;
        }

        public Task UpdatedAsync<TPart>(UpdateContentContext context, PersonPart instance)
        {
            return Task.CompletedTask;
        }
    }
}
