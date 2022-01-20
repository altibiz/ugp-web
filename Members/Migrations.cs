using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Members.Persons;
using Members.Core;
using Members.Payments;
using Members.Indexes;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Media.Settings;
using OrchardCore.ContentFields.Settings;
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
            return 6;
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

        public async Task<int> UpdateFrom6()
        {

            await _recipeMigrator.ExecuteAsync("laocalization.recipe.json", this);

            return 7;
        }
    }
}