using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System.Linq;
using Members.Core;
using Members.Utils;
using System.Threading.Tasks;

namespace Members.Indexes
{
    public class OfferIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string CompanyContentItemId { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public bool Latest { get; set; }
        public string Owner { get; set; }

    }
    public class OfferIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<OfferIndex>()
                .Map(contentItem =>
                {
                    var offer = contentItem.AsReal<Offer>();
                    if (offer == null) return null;
                    var offerIndex = new OfferIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        CompanyContentItemId = offer.Company?.ContentItemIds.FirstOrDefault(),
                        Title = contentItem.DisplayText,
                        Published = contentItem.Published,
                        Latest = contentItem.Latest,
                        Owner = contentItem.Owner
                    };
                    return offerIndex;
                });
        }
    }
    public static class OfferIndexExtensions
    {
        public static async Task CreateOfferIndex(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.CreateMapIndexTableAsync<OfferIndex>(table => table
                .Column<string>(nameof(OfferIndex.ContentItemId), c => c.WithLength(50))
                .Column<string>(nameof(OfferIndex.CompanyContentItemId), c => c.WithLength(50))
                .Column<string>(nameof(OfferIndex.Title), c => c.WithLength(225))
                .Column<string>(nameof(OfferIndex.Owner), c => c.WithLength(225))
                .Column<bool>(nameof(OfferIndex.Published))
                .Column<bool>(nameof(OfferIndex.Latest))
            );
        }
    }

}
