using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOutputCache(c => c.AddBasePolicy(o => o.Cache().Expire(TimeSpan.FromMinutes(1))));
builder.Services.AddOrchardCms()
#if DEBUG
                .AddSetupFeatures("OrchardCore.AutoSetup")
#else
                .AddAzureShellsConfiguration() //put shells info into blob
#endif
;

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequiredLength = 6;
});

var app = builder.Build();
app.UseOutputCache();
app.UseOrchardCore();

app.Run();
