using OrchardCore.ContentManagement;
using YesSql.Indexes;
using YesSql.Sql;
using System;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement.Metadata;
using Members.Utils;
using OrchardCore.Data;
using Members.Core;
using Members.Base;
using System.Threading.Tasks;

namespace Members.Persons
{
    public class PersonPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Oib { get; set; }
        public string LegalName { get; set; }
        public decimal? Revenue2019 { get; set; }
        public string PersonType { get; set; }
        public decimal? Employees { get; set; }
        public decimal? Associates { get; set; }
        public bool Published { get; set; }
        public DateTime? MembershipExpiry { get; set; }
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
                    var pp = contentItem.AsReal<PersonPart>();
                    if (pp == null) return null;
                    // Lazy initialization because of ISession cyclic dependency
                    contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();
                    var typeDef = Task.Run(async () => await contentDefinitionManager.GetSettings<PersonPartSettings>(pp)).Result;
                    var res = new PersonPartIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        Oib = pp.Oib.Text,
                        LegalName = pp.LegalName,
                        PersonType = typeDef.Type?.ToString(),
                        Published = contentItem.Published,
                        MembershipExpiry = pp.MembershipExpiry.Value,
                    };

                    var company = contentItem.AsReal<Company>();

                    if (company != null)
                    {
                        res.Revenue2019 = company.Revenue2019?.Value;
                        res.Employees = company.EmployeeNumber?.Value;
                        res.Associates = company.PermanentAssociates?.Value;
                    }

                    return res;
                });
        }
    }

    public static class PersonPartIndexExtensions
    {
        public static async Task MigratePersonPartIndex(this ISchemaBuilder SchemaBuilder)
        {
            await SchemaBuilder.CreateMapIndexTableAsync<PersonPartIndex>(table => table
                .Column<string>("Oib", col => col.WithLength(50))
                .Column<string>("ContentItemId", c => c.WithLength(50))
                .Column<string>("LegalName", c => c.WithLength(255))
                .Column<string>("PersonType", c => c.WithLength(50))
                .Column<decimal?>("Revenue2019")
                .Column<decimal?>("Employees")
                .Column<decimal?>("Associates")
                .Column<bool>("Published")
               );

            await SchemaBuilder.AlterIndexTableAsync<PersonPartIndex>(table => table
                .CreateIndex("IDX_PersonPartIndex_DocumentId",
                    "DocumentId",
                    "Oib",
                    "ContentItemId")
            );
        }

        public static async Task AddPublished(this ISchemaBuilder schemaBuilder)
        {
            await schemaBuilder.AlterIndexTableAsync<PersonPartIndex>(table => table
            .AddColumn<bool>("Published"));
            schemaBuilder.ExecuteSql("UPDATE ppi SET ppi.Published=cii.Published FROM PersonPartIndex ppi INNER JOIN " +
"ContentItemIndex cii ON ppi.DocumentId=cii.DocumentId");
        }

        public static async Task AddMembershipExpiry(this ISchemaBuilder schemaBuilder)
        {
            await schemaBuilder.AlterIndexTableAsync<PersonPartIndex>(table => table
            .AddColumn<bool>("MembershipExpiry"));
        }
    }
}
