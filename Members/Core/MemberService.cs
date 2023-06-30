using Castle.Core.Internal;
using CsvHelper;
using GraphQL;
using Members.Base;
using Members.Indexes;
using Members.Models;
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
using OrchardCore.Taxonomies.Models;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<ContentItem>GetCompanyMember(ContentItem company)
        {
            return await _session.GetListItemParent(company);
        }

        public async Task<ContentItem> GetUserMember(bool includeDraft = false, ClaimsPrincipal cUSer = null)
        {
            var user = await GetCurrentUser(cUSer);
            var query = _session.Query<ContentItem, UserPickerFieldIndex>(x => x.ContentType == nameof(Member) && x.SelectedUserId == user.UserId);
            if (!includeDraft) query = query.Where(x => x.Published);
            var member = await query.ListAsync();
            return member.FirstOrDefault();
        }

        public async Task<IEnumerable<ContentItem>> GetAllMembersForExport(DateTime startDate, string county=null, int pageIndex = 0, int pageSize = 100)
        {
            var query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == nameof(Member)).Where(x => x.Published && x.Latest);
            if (startDate <= DateTime.Now.Date) query = query.Where(x => x.PublishedUtc >= startDate);

            if (!string.IsNullOrEmpty(county))
                query = query.Where(x => x.As<PersonPart>().County.TermContentItemIds.Contains(county));

            query = (IQuery<ContentItem, ContentItemIndex>)query.Skip(pageIndex * pageSize).Take(pageSize);

            var members = await query.ListAsync();

            return members;
        }

        public async Task<IEnumerable<ContentItem>> GetAllCompaniesForExport(DateTime startDate, string county=null, string[] activity=null, int pageIndex = 0, int pageSize = 100)
        {
            var query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == nameof(Company)).Where(x => x.Published && x.Latest);

            if (startDate < DateTime.Now.Date) 
                query = query.Where(x => x.PublishedUtc >= startDate);


            if (!string.IsNullOrEmpty(county))
                query = query.Where(x => x.As<PersonPart>().County.TermContentItemIds.Contains(county));

            var companies = await query.ListAsync();

            if (activity!=null && activity.Any(a => !string.IsNullOrEmpty(a)))
                companies = companies.Where(x => activity.All(a => x.As<Company>().Activity.TermContentItemIds.Contains(a)))
                            .GroupBy(x => x.As<PersonPart>().Email?.Text)
                            .Select(x => x.FirstOrDefault())
                            .ToList();

            var pagedCompanies = companies.Skip(pageIndex * pageSize).Take(pageSize);

            return pagedCompanies;
        }

        //get's all members companies published after the date 
        public async Task<IEnumerable<ContentItem>> GetMemberCompanies(ContentItem member)
        {
            var companies = await _oHelper.QueryListItemsAsync(member.ContentItemId, x => x.ContentType == nameof(ContentType.Company));
            return companies.ToList();
        }

        public async Task<IEnumerable<ContentItem>> GetOnlyNewCompanies(DateTime afterDate)
        {

            List<ContentItem> newCompanies=new List<ContentItem>();

            var query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == nameof(Company)).Where(x => x.Published && x.Latest && x.PublishedUtc > afterDate);
            var companies = await query.ListAsync();
            companies = companies.GroupBy(x => x.As<PersonPart>().Email?.Text).Select(x => x.FirstOrDefault());

            foreach (ContentItem item  in companies)
            {
                    newCompanies.Add(item);
            }

            return newCompanies;
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
                PersonPart mem = (await GetUserMember(true)).As<PersonPart>();
                contentItem.Alter<PersonPart>(x =>
                {
                    x.Address = mem.Address;
                    x.County = mem.County;
                    x.City = mem.City;
                });
            }
            var model = await _contentItemDisplayManager.BuildEditorAsync(contentItem, _updateModelAccessor.ModelUpdater, true);
            return (contentItem, model);
        }

        public async Task<(ContentItem, IShape)> ModelToNew(ContentType memberType)
        {
            return await ModelToNew(memberType.ToString());
        }
        //get contentItem oftype taxonomy with terms
        public async Task<ContentItem> GetTaxonomy(string taxonomyName)
        {
            var query = _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == "Taxonomy" && x.DisplayText == taxonomyName);
            var taxonomy = await query.ListAsync();
            return taxonomy.FirstOrDefault();
        }

        //get taxonomy terms
        public async Task<IEnumerable<ContentItem>> GetTaxonomyTerms(string taxonomyName)
        {
            var taxonomy = await GetTaxonomy(taxonomyName);

            return taxonomy.As<TaxonomyPart>().Terms;
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


        public async Task WriteMembersToCsv(DateTime date, string exportCounty, CsvWriter csvWriter, int pageSize)
        {
            var pageIndex = 0;

            while (true)
            {
                var memList = await GetAllMembersForExport(date, exportCounty, pageIndex, pageSize);

                foreach (var item in memList)
                {
                    var member = item.As<Member>();
                    var person = item.As<PersonPart>();

                    var county = StripCounty((await person.County.GetTerm(_httpContextAccessor.HttpContext))?.DisplayText ?? "");
                    var gender = StripGender((await member.Sex.GetTerm(_httpContextAccessor.HttpContext))?.DisplayText ?? "");
                    DateTime? birthdate = member.DateOfBirth?.Value;

                    var memberCsv = new CsvModel
                    {
                        email = person.Email?.Text,
                        ime = person.Name?.Text,
                        prezime = person.Surname?.Text,
                        tvrtka = "",
                        datum_rodjenja = birthdate.HasValue ? birthdate.Value.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "",
                        djelatnost = "",
                        spol = gender,
                        tip_korisnika = "Fizičke",
                        gsm = person.Phone?.Text,
                        zupanija = county,
                        mjesto = person.City?.Text,
                        oib = person.Oib?.Text
                    };

                    if (!string.IsNullOrEmpty(memberCsv.email))
                    {
                        csvWriter.NextRecord();
                        csvWriter.WriteRecord(memberCsv);
                    }
                }

                if (memList.Count() < pageSize)
                {
                    break;
                }

                pageIndex++;
            }
        }

        public async Task WriteCompaniesToCsv(DateTime date, string exportCounty, string[] exportActivity, CsvWriter csvWriter, int pageSize)
        {
            var pageIndex = 0;

            while (true)
            {
                var onlyNewCompanies = await GetAllCompaniesForExport(date, exportCounty, exportActivity, pageIndex, pageSize);

                foreach (var item in onlyNewCompanies)
                {
                    var csv = await CompanyToCsvModelAsync(item);
                    if (csv == null || string.IsNullOrEmpty(csv.email)) continue;

                    csvWriter.NextRecord();
                    csvWriter.WriteRecord(csv);
                }

                if (onlyNewCompanies.Count() < pageSize)
                {
                    break;
                }

                pageIndex++;
            }
        }
        public async Task<CsvModel> CompanyToCsvModelAsync(ContentItem company, ContentItem member = null)
        {

            if (member == null)
            {
                member = await GetCompanyMember(company);
                if (member == null) return null;
            }

            var mpart = member.As<Member>();
            var ppart = member.As<PersonPart>();
            var cppart = company.As<PersonPart>();
            var compart = company.As<Company>();

            DateTime? birthdate = mpart?.DateOfBirth?.Value;

            var county = StripCounty((await cppart.County.GetTerm(_httpContextAccessor.HttpContext))?.DisplayText ?? "");
            var gender = StripGender((await mpart.Sex.GetTerm(_httpContextAccessor.HttpContext))?.DisplayText ?? "");

            var activityTerms = await compart.Activity?.GetTerms(_httpContextAccessor.HttpContext);

            CsvModel cs = new CsvModel();

            cs.email = cppart.Email?.Text;
            cs.ime = ppart.Name?.Text;
            cs.prezime = ppart.Surname?.Text;
            cs.tvrtka = cppart.Name?.Text;
            cs.datum_rodjenja = birthdate.HasValue ? birthdate.Value.Date.ToString("yyyy-MM-dd", new CultureInfo("hr-HR")) : "";
            cs.djelatnost = string.Join(", ", activityTerms?.Select(x => x?.DisplayText));
            cs.spol = gender;
            cs.tip_korisnika = "Pravne";
            cs.gsm = cppart.Phone?.Text;
            cs.zupanija = county;
            cs.mjesto = cppart.City?.Text;
            cs.oib = cppart.Oib?.Text;
            return cs;
        }
        public string StripCounty(string county)
        {
            string str = county.ToUpper();
            str = str.Replace("ŽUPANIJA", "");
            str = str.Replace("Ž", "Z");
            str = str.Replace("Ć", "C");
            str = str.Replace("Č", "C");
            str = str.Replace("Đ", "D");
            str = str.Replace("Š", "S");
            str = str.Trim();
            return str;
        }
        public string StripGender(string county)
        {
            string str = county.ToUpper();
            str = str.Replace("MUŠKO", "M");
            str = str.Replace("ŽENSKO", "F");
            str = str.Trim();
            return str;
        }

    }
}