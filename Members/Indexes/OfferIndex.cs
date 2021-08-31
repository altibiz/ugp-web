using Members.Core;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesSql.Indexes;
using YesSql.Sql;

namespace Members.Indexes
{
    public class OfferIndex : MapIndex
    {
            public string ContentItemId { get; set; }
            public string OfferContentItemId { get; set; }
            public string DisplayText { get; set; }

    }


    public class OfferIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<OfferIndex>()
                .Map(contentItem =>
                {
                    return new OfferIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        DisplayText = contentItem.DisplayText
                    };
                });
        }
    }
    public static class OfferIndexExtensions
    {
        public static void CreateOfferIndex(this ISchemaBuilder SchemaBuilder)
        {
            SchemaBuilder.CreateMapIndexTable<OfferIndex>(table => table
                .Column<string>(nameof(OfferIndex.ContentItemId), c => c.WithLength(26))
                .Column<string>(nameof(OfferIndex.DisplayText), c => c.WithLength(26))
            );
        }
    }


}
