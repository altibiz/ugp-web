using Members.Indexes;
using Members.Payments;
using Members.Utils;
using Microsoft.AspNetCore.Http;
using OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Taxonomies.Indexing;
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
        Company,
        Offer
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

        public MemberService(ISession session, IUserService userService, IContentManager contentManager, IOrchardHelper orchardHelper,
            IContentItemDisplayManager contentItemDisplayManager, IUpdateModelAccessor updateModelAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _session = session;
            _oHelper = orchardHelper;
            _contentManager = contentManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ContentItem> GetUserMember(bool includeDraft = false, ClaimsPrincipal cUSer = null)
        {
            var user = await GetCurrentUser(cUSer);
            var query = _session.Query<ContentItem, UserPickerFieldIndex>();
            query = query.With<UserPickerFieldIndex>(x => x.ContentType == nameof(Member) && (x.Published || includeDraft) && x.SelectedUserId == user.UserId);
            var member = await query.ListAsync();
            return member.FirstOrDefault();
        }
        public async Task<List<ContentItem>> GetUserCompanies()
        {
            ContentItem member = await GetUserMember();
            var companyContentItem = new List<ContentItem>();

            var companies = await _oHelper.QueryListItemsAsync(member.ContentItemId);
            companies = companies.Where(x => x.ContentType == nameof(ContentType.Company));

            return companies.ToList();
        }

        public async Task<ContentItem> GetContentItemById(string contentItemId)
        {
            var query = _session.Query<ContentItem>();
            query = query.With<ContentItemIndex>(x => x.ContentItemId == contentItemId);
            var ci = await query.ListAsync();
            return ci.FirstOrDefault();
        }

        public async Task<ContentItem> GetContentItemOffers(string contentItemId, bool includeDraft = false)
        {


            var company = await GetContentItemById(contentItemId);

            var query = _session.Query<ContentItem, ContentPickerFieldIndex>();
            query = query.With<ContentPickerFieldIndex>(x => x.ContentType == nameof(Offer) && (x.Published || includeDraft) && x.SelectedContentItemId == company.ContentItemId);
            var member = await query.ListAsync();
            return member.FirstOrDefault();

        }

        public async Task<ContentItem> GetCompanyOffers(string companyContentItemId, bool includeDraft = false)
        {


            var company = await GetContentItemById(companyContentItemId);

            var query = _session.Query<ContentItem, OfferIndex>();
            query = query.With<OfferIndex>(x => (x.Published || includeDraft) && x.CompanyContentItemId == company.ContentItemId);
            var member = await query.ListAsync();
            return member.FirstOrDefault();

        }
        internal async IAsyncEnumerable<Payment> GetUserPayments()
        {
            var member = await GetUserMember();
            var companies = await GetUserCompanies();
            foreach (var payment in await GetPersonPayments(member.ContentItemId))
                yield return payment.As<Payment>();
            foreach (var comp in companies)
                foreach (var payment in await GetPersonPayments(comp.ContentItemId))
                    yield return payment.As<Payment>();
        }

        internal async Task<IEnumerable<ContentItem>> GetPersonPayments(string contentItemId)
        {
            return await _session.Query<ContentItem, PaymentIndex>(x => x.PersonContentItemId == contentItemId).ListAsync();
        }
		
        private async Task<User> GetCurrentUser(ClaimsPrincipal user = null)
        {
            return await _userService.GetAuthenticatedUserAsync(user ?? _httpContextAccessor.HttpContext.User) as User;
        }

        public async Task<(ContentItem, IShape)> GetNewItem(ContentType cType)
        {
            var contentItem = await _contentManager.NewAsync(cType.ToString());
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            return (contentItem, model);
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

        public async Task<(IShape,ContentItem)> GetEditorById(string contentId)
        {
            var contentItem = await _contentManager.GetAsync(contentId,VersionOptions.Latest);

            var shape= await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);
            return (shape, contentItem);
        }
        public async Task<ContentValidateResult> CreateMemberCompany(ContentItem companyItem)
        {
            var member = await GetUserMember(true);
            if (member == null) return new ContentValidateResult { Succeeded = false };
            member.AddToList(companyItem);
            return await _contentManager.UpdateValidateAndCreateAsync(companyItem, VersionOptions.Draft);
        }

        public async Task<ContentValidateResult> UpdateContentItem(ContentItem contentItem)
        {
            await _contentManager.UpdateAsync(contentItem);
            return await _contentManager.ValidateAsync(contentItem);
        }

        public async Task<ContentValidateResult> CreateMemberDraft(ContentItem memberItem)
        {

            var user = await GetCurrentUser();
            // Set the current user as the owner to check for ownership permissions on creation
            memberItem.Owner = user.UserName;
            memberItem.Alter<Member>(member =>
            {
                member.User.UserIds = new[] { user.UserId };
            });

            return await _contentManager.UpdateValidateAndCreateAsync(memberItem, VersionOptions.Draft);
        }

        public async Task<ContentValidateResult> CreateOfferDraft(ContentItem offerItem, string parentContentItemId)
        {
            var parentContentItem = await GetContentItemById(parentContentItemId);
            if (parentContentItem == null) return new ContentValidateResult { Succeeded = false };

            // DORADITI !!!!
            offerItem.Alter<Offer>(offer =>
            {
                 offer.Company.ContentItemIds = new[] { parentContentItem.ContentItemId };
            });

            return await _contentManager.UpdateValidateAndCreateAsync(offerItem, VersionOptions.Draft);
        }

        public async Task<List<ContentItem>> GetAllOffers()
        {
            var query = _session.Query<ContentItem, ContentItemIndex>();
            query = query.With<ContentItemIndex>(x => x.ContentType == nameof(Offer) && x.Published );

            var list = await query.ListAsync();

            return list.ToList();
        }
        public async Task<List<ContentItem>> GetAllOffersByTag(string tagId)
        {
            var query = _session.Query<ContentItem, TaxonomyIndex>();
            query = query.With<TaxonomyIndex>(x => x.ContentType == nameof(Offer) && x.Published && x.TermContentItemId.Contains(tagId));

            var list = await query.ListAsync();

            return list.ToList();
        }
        public async Task<List<ContentItem>> GetAllOffersSearch(string searchString)
        {
            var query = _session.Query<ContentItem, OfferIndex>();
            query = query.With<OfferIndex>(x => x.Published && x.DisplayText.Contains(searchString));

            var list = await query.ListAsync();

            return list.ToList();
        }
    }
}