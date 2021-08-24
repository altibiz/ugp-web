using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using Members.Persons;
using Members.Core;
using Members.Payments;

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

            return 1;
        }
    }
}