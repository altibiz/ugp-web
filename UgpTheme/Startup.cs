using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Taxonomies.Drivers;
using OrchardCore.Taxonomies.Fields;
using System;
using UgpTheme.Drivers;

namespace OrchardCore.Themes.UgpTheme
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
            serviceCollection.AddScoped<IDataMigration, Migrations>();

            serviceCollection.AddContentField<TaxonomyField>().UseDisplayDriver<TaxonomyFieldTagsDisplayDriver>(d => false)
                .UseDisplayDriver<TaxonomyFieldDisplayDriver>(d => false)
                .UseDisplayDriver<TxnFieldDisplayDriver>(d => !string.Equals(d, "Tags", StringComparison.OrdinalIgnoreCase))
                .UseDisplayDriver<TxnFieldTagsDisplayDriver>(d => string.Equals(d, "Tags", StringComparison.OrdinalIgnoreCase));
        }
    }
}
