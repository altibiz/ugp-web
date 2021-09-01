using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Navigation;
using Microsoft.Extensions.Hosting;
using OrchardCore.DisplayManagement.Implementation;
using Members.Utils;
using OrchardCore.ContentManagement;
using Members.Core;
using OrchardCore.ContentTypes.Editors;
using Lombiq.HelpfulExtensions.Extensions.CodeGeneration;
using Members.Persons;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Drivers;
using Members.PartFieldSettings;
using OrchardCore.Data;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Drivers;
using Members.Payments;
using YesSql.Indexes;

namespace Members
{
    public class Startup : OrchardCore.Modules.StartupBase
    {
        public IWebHostEnvironment CurrentEnvironment { get; }

        public Startup(IWebHostEnvironment env)
        {
            CurrentEnvironment = env;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddContentPart<Member>();
            services.UsePartService<PersonPart, PersonPartService>();
            services.AddScoped<MemberService>();
            services.AddScoped<IScopedIndexProvider, PersonPartIndexProvider>();
            services.AddSingleton<IIndexProvider, PaymentIndexProvider>();
            services.AddContentPart<Payment>();
            if (CurrentEnvironment.IsDevelopment()) 
            {
                services.AddScoped<IShapeDisplayEvents, ShapeTracingShapeEvents>();
                services.AddScoped<IContentTypeDefinitionDisplayDriver, CodeGenerationDisplayDriver>();
            }

            services.AddContentField<TextField>().ForEditor<TextFieldDisplayDriver>(d => false)
                .ForEditor<PartTextFieldDriver>(d=>true);

            services.AddContentField<TaxonomyField>().ForEditor<TaxonomyFieldTagsDisplayDriver>(d => false)
                .ForEditor<TaxonomyFieldDisplayDriver>(d=>!string.Equals(d, "Tags", StringComparison.OrdinalIgnoreCase) && !string.Equals(d, "Disabled", StringComparison.OrdinalIgnoreCase))
                .ForEditor<PartTaxonomyFieldTagsDriver>(d =>
                {
                    return string.Equals(d, "Tags", StringComparison.OrdinalIgnoreCase) || string.Equals(d, "Disabled", StringComparison.OrdinalIgnoreCase);
                });


        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            //routes.MapAreaControllerRoute(
            //    name: "Home",
            //    areaName: "Members",
            //    pattern: "Home/Index",
            //    defaults: new { controller = "Home", action = "Index" }
            //);
        }
    }
}
