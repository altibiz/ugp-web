using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OrchardCore.Users;
using OrchardCore.Users.Handlers;

namespace Members.Users
{
    /// <summary>
    /// The username is always the email address, also for users registering through
    /// an external provider (e.g. Facebook). Returning null falls back to
    /// OrchardCore's generated username if the provider supplied no email claim.
    /// </summary>
    public class EmailExternalLoginEventHandler : IExternalLoginEventHandler
    {
        public Task<string> GenerateUserName(string provider, IEnumerable<SerializableClaim> claims)
        {
            var email = claims?
                .FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?
                .Value;

            return Task.FromResult(email);
        }

        public Task UpdateUserAsync(UpdateUserContext context) => Task.CompletedTask;
    }
}
