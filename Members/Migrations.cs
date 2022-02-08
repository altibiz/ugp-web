using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Members.Persons;
using Members.Core;
using Members.Payments;
using Members.Indexes;
using Members.Base;
using YesSql;
using Members.Utils;

namespace Members
{
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager cdf)
        {
            _recipeMigrator = recipeMigrator;
            _contentDefinitionManager = cdf;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("init.recipe.json", this);

            #region PersonPart
            _contentDefinitionManager.AlterPersonPart();
            SchemaBuilder.MigratePersonPartIndex();
            #endregion

            _contentDefinitionManager.ExecuteMemberMigrations();
            _contentDefinitionManager.MigratePayment();
            SchemaBuilder.CreatePaymentIndex();
            _contentDefinitionManager.MigrateOffer();
            SchemaBuilder.CreateOfferIndex();
            _contentDefinitionManager.CreateBankStatement();
            await _recipeMigrator.ExecuteAsync("pledge.recipe.json", this);
            _contentDefinitionManager.CreatePledge();
            _contentDefinitionManager.DefineImageBanner();
            SchemaBuilder.AddPayoutField();
            SchemaBuilder.AddPaymentPublished();
            _contentDefinitionManager.AdminPage();
            SchemaBuilder.AddTransactionRef();
            SchemaBuilder.CreatePaymentByDayIndex();
            return 12;
        }

        public int UpdateFrom1()
        {
            _contentDefinitionManager.AlterPartDefinition("Offer", part => part
                .RemoveField("LongDescription")//remove to add
                .RemoveField("NestoTamo")//remove to add
                );
            _contentDefinitionManager.MigrateOffer();
            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.AddPublished();
            return 3;
        }

        public async Task<int> UpdateFrom3()
        {
            await _recipeMigrator.ExecuteAsync("pledge.recipe.json", this);
            _contentDefinitionManager.CreatePledge();
            return 4;
        }

        public int UpdateFrom4()
        {
            _contentDefinitionManager.DefineImageBanner();
            return 5;
        }

        public int UpdateFrom5()
        {
            _contentDefinitionManager.MigratePayment();
            return 6;
        }

        public int UpdateFrom6()
        {
            SchemaBuilder.AddPayoutField();
            return 7;
        }

        public int UpdateFrom7()
        {
            _contentDefinitionManager.MigratePayment();
            return 8;
        }

        public int UpdateFrom8()
        {
            SchemaBuilder.AddPaymentPublished();
            return 9;
        }

        public int UpdateFrom9()
        {
            _contentDefinitionManager.AdminPage();
            return 10;
        }

        public int UpdateFrom10()
        {
            SchemaBuilder.AddTransactionRef();
            return 11;
        }

        public int UpdateFrom11()
        {
            SchemaBuilder.CreatePaymentByDayIndex();
            return 12;
        }
    }
}