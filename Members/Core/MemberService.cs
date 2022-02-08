using Members.Base;
using Members.Indexes;
using Members.Persons;
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
        private readonly IUserService _userService;
        public ISession _session;

        public IContentManager _contentManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
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
            var query = _session.Query<ContentItem, UserPickerFieldIndex>(x => x.ContentType == nameof(Member) && x.SelectedUserId == user.UserId);
            if (!includeDraft) query = query.Where(x => x.Published);
            var member = await query.ListAsync();
            return member.FirstOrDefault();
        }
        public async Task<List<ContentItem>> GetUserCompanies()
        {
            ContentItem member = await GetUserMember();
            var companyContentItem = new List<ContentItem>();

            var companies = await _oHelper.QueryListItemsAsync(member.ContentItemId, x => true);
            companies = companies.Where(x => x.ContentType == nameof(ContentType.Company));
            return companies.ToList();
        }

        public async Task<ContentItem> GetContentItemById(string contentItemId)
        {
            return await _session.GetItemById(contentItemId);
        }

        public async Task<ContentItem> GetByOib(string oib)
        {
            return await _session.Query<ContentItem>().With<PersonPartIndex>(x => x.Oib == oib).FirstOrDefaultAsync();
        }

        public async Task<ContentItem> GetCompanyOffers(string companyContentItemId, bool includeDraft = false)
        {
            var company = await GetContentItemById(companyContentItemId);

            var query = _session.Query<ContentItem, OfferIndex>(x => x.CompanyContentItemId == company.ContentItemId);
            if (!includeDraft)
                query = query.Where(x => x.Published == true);
            var member = await query.ListAsync();
            return member.FirstOrDefault();

        }

        private async Task<User> GetCurrentUser(ClaimsPrincipal user = null)
        {
            return await _userService.GetAuthenticatedUserAsync(user ?? _httpContextAccessor.HttpContext.User) as User;
        }

        public async Task<(ContentItem, IShape)> GetNewItem(ContentType cType)
        {
            return await GetNewItem(cType.ToString());
        }

        public async Task<(ContentItem, IShape)> GetNewItem(string cType)
        {
            var contentItem = await _contentManager.NewAsync(cType.ToString());
            if (cType.Equals(ContentType.Company))
            {
                ContentItem mem = await GetUserMember(true);
                contentItem.Content.PersonPart.Address = mem.Content.PersonPart.Address;
                contentItem.Content.PersonPart.County = mem.Content.PersonPart.County;
                contentItem.Content.PersonPart.City = mem.Content.PersonPart.City;
            }
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            return (contentItem, model);
        }

        public async Task<(ContentItem, IShape)> ModelToNew(ContentType memberType)
        {
            return await ModelToNew(memberType.ToString());
        }

        public async Task<(ContentItem, IShape)> ModelToNew(string memberType)
        {
            return await ModelToItem(await _contentManager.NewAsync(memberType));
        }

        public async Task<(ContentItem, IShape)> ModelToItem(string id = null)
        {
            return await ModelToItem(await _contentManager.GetAsync(id, VersionOptions.Latest));
        }

        public async Task<(ContentItem, IShape)> ModelToItem(ContentItem contentItem)
        {
            var shape = await _contentItemDisplayManager.UpdateEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);

            if (!_updateModelAccessor.ModelUpdater.ModelState.IsValid)
            {
                await _session.CancelAsync();
            }
            return (contentItem, shape);
        }

        public async Task<(IShape, ContentItem)> GetEditorById(string contentId)
        {
            var contentItem = await _contentManager.GetAsync(contentId, VersionOptions.Latest);

            var shape = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, false);
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

        public async Task<ContentValidateResult> CreateNew(ContentItem memberItem, bool published = false)
        {
            return await _contentManager.UpdateValidateAndCreateAsync(memberItem, published ? VersionOptions.Published : VersionOptions.Draft);
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
            var query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == nameof(Offer) && x.Published);

            var list = await query.ListAsync();

            return list.ToList();
        }
        public async Task<List<ContentItem>> GetAllOffersByTag(string tagId)
        {
            var query = _session.Query<ContentItem, TaxonomyIndex>(x => x.ContentType == nameof(Offer) && x.Published && x.TermContentItemId.Contains(tagId));

            var list = await query.ListAsync();

            return list.ToList();
        }
        public async Task<List<ContentItem>> GetAllOffersSearch(string searchString)
        {
            var query = _session.Query<ContentItem, OfferIndex>(x => x.Published && x.Title.Contains(searchString));

            var list = await query.ListAsync();

            return list.ToList();
        }
    }
}