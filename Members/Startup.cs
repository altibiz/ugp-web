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
using Members.Models;
using OrchardCore.ContentTypes.Editors;
using Lombiq.HelpfulExtensions.Extensions.CodeGeneration;
using YesSql.Indexes;
using Members.Indexes;
using Members.Handlers;
using Members.Persons;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Drivers;

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
            services.AddContentPart<PersonPart>().AddHandler<PersonPartHandler>()
                .UseDisplayDriver<PersonPartDisplayDriver>();
            services.AddSingleton<IIndexProvider, PersonPartIndexProvider>();
            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddScoped<IShapeDisplayEvents, ShapeTracingShapeEvents>();
                services.AddScoped<IContentTypeDefinitionDisplayDriver, CodeGenerationDisplayDriver>();
            }

            services.AddContentField<TextField>().ForEditor<TextFieldDisplayDriver>(d => d== "MyCustomEditor")
                .ForEditor<MembersTextFieldDriver>(d=>d!="MyCustomEditor");

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
