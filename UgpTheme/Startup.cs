using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;
using Microsoft.AspNetCore.Mvc;
using UgpTheme;

namespace OrchardCore.Themes.UgpTheme
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
            serviceCollection.AddScoped<IDataMigration, Migrations>();
        }
    }
}
