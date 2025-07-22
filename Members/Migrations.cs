using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Members.Persons;
using Members.Core;
using Members.Payments;
using Members.Indexes;
using Members.Base;

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
            await _contentDefinitionManager.AlterPersonPart();
            await SchemaBuilder.MigratePersonPartIndex();
            #endregion

            await _contentDefinitionManager.ExecuteMemberMigrations();
            await _contentDefinitionManager.MigratePayment();
            await SchemaBuilder.CreatePaymentIndex();
            await _contentDefinitionManager.MigrateOffer();
            await SchemaBuilder.CreateOfferIndex();
            await _contentDefinitionManager.CreateBankStatement();
            await _recipeMigrator.ExecuteAsync("pledge.recipe.json", this);
            await _contentDefinitionManager.CreatePledge();
            await _contentDefinitionManager.DefineImageBanner();
            await SchemaBuilder.AddPayoutField();
            await SchemaBuilder.AddPaymentPublished();
            await _contentDefinitionManager.AdminPage();
            await SchemaBuilder.AddTransactionRef();
            await SchemaBuilder.CreatePaymentByDayIndex();
            return 13;
        }

        public async Task<int> UpdateFrom1()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("Offer", part => part
                .RemoveField("LongDescription")//remove to add
                .RemoveField("NestoTamo")//remove to add
                );
            await _contentDefinitionManager.MigrateOffer();
            return 2;
        }

        public async Task<int> UpdateFrom2()
        {
            await SchemaBuilder.AddPublished();
            return 3;
        }

        public async Task<int> UpdateFrom3()
        {
            await _recipeMigrator.ExecuteAsync("pledge.recipe.json", this);
            await _contentDefinitionManager.CreatePledge();
            return 4;
        }

        public async Task<int> UpdateFrom4()
        {
            await _contentDefinitionManager.DefineImageBanner();
            return 5;
        }

        public async Task<int> UpdateFrom5()
        {
            await _contentDefinitionManager.MigratePayment();
            return 6;
        }

        public async Task<int> UpdateFrom6()
        {
            await SchemaBuilder.AddPayoutField();
            return 7;
        }

        public async Task<int> UpdateFrom7()
        {
            await _contentDefinitionManager.MigratePayment();
            return 8;
        }

        public async Task<int> UpdateFrom8()
        {
            await SchemaBuilder.AddPaymentPublished();
            return 9;
        }

        public async Task<int> UpdateFrom9()
        {
            await _contentDefinitionManager.AdminPage();
            return 10;
        }

        public async Task<int> UpdateFrom10()
        {
            await SchemaBuilder.AddTransactionRef();
            return 11;
        }

        public async Task<int> UpdateFrom11()
        {
            await SchemaBuilder.CreatePaymentByDayIndex();
            return 12;
        }

        public int UpdateFrom12()
        {
            return 13;
        }

        public async Task<int> UpdateFrom13()
        {
            await _contentDefinitionManager.UpdatePledgeForm();
            return 14;
        }

        public async Task<int> UpdateFrom14()
        {
            await _contentDefinitionManager.UpdatePledgeVariant();
            return 15;
        }

        public async Task<int> UpdateFrom15()
        {
            await SchemaBuilder.AddMembershipExpiry();
            return 16;
        }

        public async Task<int> UpdateFrom16()
        {
            await _contentDefinitionManager.AlterPersonPart();
            return 17;
        }

        public async Task<int> UpdateFrom17()
        {
            await _contentDefinitionManager.UpdatePledgeForm();
            return 18;
        }
    }
}