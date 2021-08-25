using Members.Payments;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YesSql;
using ISession = YesSql.ISession;

namespace Members.Core
{
    public enum ContentType
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
        private readonly IOrchardHelper _oHelper;

        public MemberService(ISession session, IUserService userService,IContentManager contentManager, IOrchardHelper orchardHelper,
            IContentItemDisplayManager contentItemDisplayManager,IUpdateModelAccessor updateModelAccessor,IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _session = session;
            _oHelper = orchardHelper;
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ContentItem> GetUserMember(bool includeDraft=false,ClaimsPrincipal cUSer = null)
        {
            var user = await GetCurrentUser(cUSer);
            var query = _session.Query<ContentItem, UserPickerFieldIndex>();
            query = query.With<UserPickerFieldIndex>(x => x.ContentType == nameof(Member) && (x.Published || includeDraft) && x.SelectedUserId == user.UserId);
            var member = await query.ListAsync();
            return member.FirstOrDefault();
        }
        public async Task<List<ContentItem>> GetUserCompanies(string userMemberId)
        {
            var companyContentItem = new List<ContentItem>();
            foreach (var contentItem in await _oHelper.QueryListItemsAsync(userMemberId))
            {
                companyContentItem.Add(contentItem);
            }
            return companyContentItem;
        }

        internal async IAsyncEnumerable<Payment> GetUserPayments()
        {
            var member = await GetUserMember();
            var companies = await GetUserCompanies(member.ContentItemId);
            foreach (var payment in await GetPersonPayments(member.ContentItemId))
                yield return payment.As<Payment>();
            foreach (var comp in companies)
                foreach (var payment in await GetPersonPayments(comp.ContentItemId))
                    yield return payment.As<Payment>();
        }

        internal async Task<IEnumerable<ContentItem>> GetPersonPayments(string contentItemId)
        {
            return await _session.Query<ContentItem,PaymentIndex>(x => x.PersonContentItemId == contentItemId).ListAsync();
        }

        private async Task<User> GetCurrentUser(ClaimsPrincipal user = null)
        {
            return await _userService.GetAuthenticatedUserAsync(user??_httpContextAccessor.HttpContext.User) as User;
        }

        public async Task<(ContentItem,IShape)> GetNewItem(ContentType memberType)
        {
            var contentItem = await _contentManager.NewAsync(memberType.ToString());
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            return (contentItem,model);
        }

        public async Task<(ContentItem, IShape)> GetUpdatedItem(ContentType memberType)
        {
            return await GetUpdatedItem(await _contentManager.NewAsync(memberType.ToString()));
        }

        public async Task<(ContentItem, IShape)> GetUpdatedItem(string id = null)
        {
            return await GetUpdatedItem(await _contentManager.GetAsync(id));
        }

        public async Task<(ContentItem, IShape)> GetUpdatedItem(ContentItem contentItem)
        {
            var shape = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!_updateModelAccessor.ModelUpdater.ModelState.IsValid)
            {
                await _session.CancelAsync();
            }
            return (contentItem, shape);
        }

        public async Task<IShape> GetEditorById(string contentId)
        {
            var contentItem = await _contentManager.GetAsync(contentId);

            return await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);
        }
        public async Task<ContentValidateResult> CreateMemberCompany(ContentItem companyItem)
        {
            var member = await GetUserMember();
            if (member == null) return new ContentValidateResult { Succeeded = false };
            member.AddToList(companyItem);
            return await _contentManager.UpdateValidateAndCreateAsync(companyItem, VersionOptions.Published);
        }

        public async Task<ContentValidateResult> UpdateMemberCompany(ContentItem companyItem)
        {
            await _contentManager.UpdateAsync(companyItem);
            return await  _contentManager.ValidateAsync(companyItem);
        }

        public async Task<ContentValidateResult> CreateMemberDraft(ContentItem memberItem) { 

            var user = await GetCurrentUser();
            // Set the current user as the owner to check for ownership permissions on creation
            memberItem.Owner = user.UserName;
            memberItem.Alter<Member>(member =>
            {
                member.User.UserIds = new[] { user.UserId };
            });

            return await _contentManager.UpdateValidateAndCreateAsync(memberItem, VersionOptions.Draft);
        }
    }
}