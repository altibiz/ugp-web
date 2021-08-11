using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YesSql;
using ISession = YesSql.ISession;

namespace Members.Core
{

    public class ProfileService
    {
        private IUserService _userService;
        private ISession _session;
        private IContentManager _contentManager;
        private IContentItemDisplayManager _contentItemDisplayManager;
        private IUpdateModelAccessor _updateModelAccessor;
        private IHttpContextAccessor _httpContextAccessor;

        public ProfileService(ISession session, IUserService userService,IContentManager contentManager,
            IContentItemDisplayManager contentItemDisplayManager,IUpdateModelAccessor updateModelAccessor,IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _session = session;
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _httpContextAccessor = httpContextAccessor;
        }
        //public async Task<ContentItem> GetUserProfile(ClaimsPrincipal cUSer=null)
        //{

        //}

        private async Task<User> GetCurrentUser(ClaimsPrincipal user = null)
        {
            return await _userService.GetAuthenticatedUserAsync(user??_httpContextAccessor.HttpContext.User) as User;
        }

    }
}
