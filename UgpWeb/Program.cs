using Microsoft.AspNetCore.Identity;
#if DEBUG
using Microsoft.Data.Sqlite;
#endif

var builder = WebApplication.CreateBuilder(args);

// Workaround for OrchardCore 2.1.7 SqliteCacheMode.Shared bug.
// Pre-configure SQLite DB with WAL journal mode and busy timeout to prevent
// SQLITE_READONLY_DBMOVED errors during recipe setup with many concurrent migrations.
// See: https://github.com/OrchardCMS/OrchardCore/pull/18043
#if DEBUG
var dbFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Sites", "Default");
Directory.CreateDirectory(dbFolder);
var dbPath = Path.Combine(dbFolder, "OrchardCore.db");
using (var connection = new SqliteConnection($"Data Source={dbPath}"))
{
    connection.Open();
    using var cmd = connection.CreateCommand();
    cmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000; PRAGMA synchronous=NORMAL;";
    cmd.ExecuteNonQuery();
}
#endif

builder.Services.AddOrchardCms()
    .ConfigureServices(tenantServices =>
        tenantServices.ConfigureHtmlSanitizer((sanitizer) =>
        {
            sanitizer.AllowedTags.Add("iframe");
        })
    )
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
app.UseOrchardCore();
app.Run();
