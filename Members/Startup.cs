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
using Members.Services;
using OrchardCore.ContentManagement;
using Members.Models;

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
            services.AddScoped<MemberService>();
            services.AddContentPart<Member>();
            if(CurrentEnvironment.IsDevelopment())
                services.AddScoped<IShapeDisplayEvents, ShapeTracingShapeEvents>();

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
