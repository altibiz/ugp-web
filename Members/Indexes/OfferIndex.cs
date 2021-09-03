﻿using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System;
using System.Linq;
using Members.Core;

namespace Members.Indexes
{
    public class OfferIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string CompanyContentItemId { get; set; }
        public string DisplayText { get; set; }
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
                    var offer = contentItem.As<Offer>();
                    if (offer == null) return null;

                    var offerIndex = new OfferIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        CompanyContentItemId = contentItem.ContentItem.Content.Offer.Company.ContentItemIds[0],
                        DisplayText = contentItem.DisplayText,
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
        public static void CreateOfferIndex(this ISchemaBuilder SchemaBuilder)
        {
            SchemaBuilder.CreateMapIndexTable<OfferIndex>(table => table
                .Column<string>(nameof(OfferIndex.ContentItemId), c => c.WithLength(26))
                .Column<string>(nameof(OfferIndex.CompanyContentItemId), c => c.WithLength(26))
                .Column<string>(nameof(OfferIndex.DisplayText), c => c.WithLength(26))
                .Column<string>(nameof(OfferIndex.Owner), c => c.WithLength(26))
                .Column<bool>(nameof(OfferIndex.Published))
                .Column<bool>(nameof(OfferIndex.Latest))
            );
        }
    }

}