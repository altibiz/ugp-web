﻿using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement.Metadata;
using Members.Utils;
using OrchardCore.Data;

namespace Members.Persons
{
    public class PersonPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Oib { get; set; }
        public string LegalName { get; set; }

        public string PersonType { get; set; }
    }

    public class PersonPartIndexProvider : IndexProvider<ContentItem>, IScopedIndexProvider
    {
        private IServiceProvider _serviceProvider;
        private IContentDefinitionManager contentDefinitionManager;

        public PersonPartIndexProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<PersonPartIndex>()
                .Map(contentItem =>
                {
                    var pp = contentItem.As<PersonPart>();
                    if (pp == null) return null;
                    // Lazy initialization because of ISession cyclic dependency
                    contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();
                    var typeDef = contentDefinitionManager.GetSettings<PersonPartSettings>(pp);

                    return new PersonPartIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        Oib = pp.Oib.Text,
                        LegalName = pp.LegalName,
                        PersonType = typeDef.Type?.ToString(),
                    };
                });
        }
    }

    public static class PersonPartIndexExtensions
    {
        public static void MigratePersonPartIndex(this ISchemaBuilder SchemaBuilder)
        {
            SchemaBuilder.CreateMapIndexTable<PersonPartIndex>(table => table
                .Column<string>("Oib", col => col.WithLength(20))
                .Column<string>("ContentItemId", c => c.WithLength(26))
                .Column<string>("LegalName", c => c.WithLength(100))
                .Column<string>("PersonType",c=>c.WithLength(50))
            );

            SchemaBuilder.AlterIndexTable<PersonPartIndex>(table => table
                .CreateIndex("IDX_PersonPartIndex_DocumentId",
                    "DocumentId",
                    "Oib",
                    "ContentItemId")
            );
        }
    }
}