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
    public enum MemberType
    {
        Member,
        Company
    }

    public class MemberService
    {
        private IUserService _userService;
        private ISession _session;
        private IContentManager _contentManager;
        private IContentItemDisplayManager _contentItemDisplayManager;
        private IUpdateModelAccessor _updateModelAccessor;
        private IHttpContextAccessor _httpContextAccessor;

        public MemberService(ISession session, IUserService userService,IContentManager contentManager,
            IContentItemDisplayManager contentItemDisplayManager,IUpdateModelAccessor updateModelAccessor,IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _session = session;
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ContentItem> GetUserMember(ClaimsPrincipal cUSer=null)
        {
            var user = await GetCurrentUser(cUSer);
            var query = _session.Query<ContentItem, UserPickerFieldIndex>();
            query = query.With<UserPickerFieldIndex>(x => x.ContentType == nameof(Member) && x.Published && x.SelectedUserId == user.UserId);
            var member = await query.ListAsync();
            return member.FirstOrDefault();
        }

        private async Task<User> GetCurrentUser(ClaimsPrincipal user = null)
        {
            return await _userService.GetAuthenticatedUserAsync(user??_httpContextAccessor.HttpContext.User) as User;
        }

        public async Task<(ContentItem,IShape)> GetNewItem(MemberType memberType)
        {
            var contentItem = await _contentManager.NewAsync(memberType.ToString());
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            return (contentItem,model);
        }

        public async Task<(ContentItem, IShape)> GetUpdatedItem(MemberType memberType)
        {
            var contentItem = await _contentManager.NewAsync(memberType.ToString());
            var shape = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!_updateModelAccessor.ModelUpdater.ModelState.IsValid)
            {
                await _session.CancelAsync();
            }
            return (contentItem, shape);
        }

        public async Task<ContentValidateResult> CreateMemberCompany(ContentItem companyItem)
        {
            var member = await GetUserMember();
            if (member == null) return new ContentValidateResult { Succeeded = false };
            member.AddToList(companyItem);
            return await _contentManager.UpdateValidateAndCreateAsync(companyItem, VersionOptions.Published);
        }

        public async Task<ContentValidateResult> CreateMember(ContentItem memberItem) { 

            var user = await GetCurrentUser();
            // Set the current user as the owner to check for ownership permissions on creation
            memberItem.Owner = user.UserName;
            memberItem.Alter<Member>(member =>
            {
                member.User.UserIds = new[] { user.UserId };
            });

            return await _contentManager.UpdateValidateAndCreateAsync(memberItem, VersionOptions.Published);
        }
    }
}
