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
using Members.Indexes;
using OrchardCore.Recipes;
using Members.Base;
using OrchardCore.Contents.Services;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Contents.ViewModels;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement.Handlers;
using Members.ContentHandlers;

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
            services.AddContentPart<Company>();
            services.UsePartService<PersonPart, PersonPartService>();
            services.UsePartService<BankStatPart, BankStatPartService>();
            services.AddScoped<MemberService>();
            services.AddScoped<PaymentUtils>();
            services.AddScoped<IScopedIndexProvider, PersonPartIndexProvider>();
            services.AddSingleton<IIndexProvider, PaymentIndexProvider>();
            services.AddSingleton<IIndexProvider, OfferIndexProvider>();
            services.AddSingleton<IIndexProvider, PaymentByDayIndexProvider>();
            services.AddContentPart<Payment>();
            services.AddContentPart<Offer>();
            services.AddSingleton<IContentHandler, MemberHandler>();
            services.AddSingleton<IContentHandler, UserMenuHandler>();
            services.AddRecipeExecutionStep<FastImport>();
            services.AddScoped<Importer>();
            services.AddTransient<IContentsAdminListFilterProvider, PersonPartAdminListFilterProvider>();
            services.AddTransient<IContentsAdminListFilterProvider, PaymentAdminListFilterProvider>();
            services.AddScoped<IDisplayDriver<ContentOptionsViewModel>, PersonOptionsDisplayDriver>();
            services.UsePartService<Pledge, PledgeService>();
            services.UsePartService<Payment, PaymentPartService>();
            services.AddContentPart<PledgeForm>();

            services.AddScoped<IContentDisplayDriver, ContainedPartDisplayDriver>();
            services.AddSingleton<IBackgroundTask, FastImportBackgroundTask>();
            services.AddScoped<MemberExportService>();
            services.AddSingleton<IBackgroundTask, MemberExportBackgroundTask>();
            services.AddScoped<IContentDisplayHandler, ContentDisplayHandlerExt>();

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddScoped<IShapeDisplayEvents, ShapeTracingShapeEvents>();
                services.AddScoped<IContentTypeDefinitionDisplayDriver, CodeGenerationDisplayDriver>();
            }
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
