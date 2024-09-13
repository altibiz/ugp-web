using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Settings;
using OrchardCore.Recipes.Services;
using OrchardCore.Search.Lucene.Model;
using OrchardCore.Taxonomies.Settings;
using System.Threading.Tasks;
using YesSql;

namespace OrchardCore.Themes.UgpTheme
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ISession _session;
        private readonly IRecipeMigrator _migrator;

        public Migrations(IContentDefinitionManager contentDefinitionManager, ISession session, IRecipeMigrator migrator)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _session = session;
            _migrator = migrator;
        }

        public async Task<int> Create()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("GPiece", cfg => cfg
                .WithDescription("Contains the fields for the current type")
                .WithField("Caption",
                    fieldBuilder => fieldBuilder
                        .OfType("HtmlField")
                        .WithDisplayName("Caption")
                        .WithEditor("Wysiwyg"))
                .WithField("DisplayCaption",
                    fieldBuilder => fieldBuilder
                        .OfType("BooleanField")
                        .WithDisplayName("Display Caption"))
                .WithField("Image",
                    fieldBuilder => fieldBuilder
                        .OfType("MediaField")
                        .WithDisplayName("Image")
                        .WithSettings(new MediaFieldSettings { Required = true, Multiple = false }))
                .WithField("ImageClass",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Image Class"))
                .WithField("ImageAltText",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Image Alt Text"))
            );

            await _contentDefinitionManager.AlterTypeDefinitionAsync("GPiece", type => type
                .WithPart("GPiece"));

            await _contentDefinitionManager.AlterPartDefinitionAsync("Gallery", cfg => cfg
                .WithDescription("Contains the fields for the current type")
                .WithField("DisplayType",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Display Type"))
            );

            await _contentDefinitionManager.AlterTypeDefinitionAsync("Gallery", type => type
                .WithPart("TitlePart")
                .WithPart("Gallery")
                .WithPart("GPieces", "BagPart", cfg => cfg
                    .WithDisplayName("GPieces")
                    .WithDescription("GPieces to display in the carousel.")
                    .WithSettings(new BagPartSettings { ContainedContentTypes = new[] { "GPiece" }, DisplayType = "Detail" }))
                .Stereotype("Widget"));

            return 1;
        }

        public async Task<int> UpdateFrom1()
        {
            var ci = await _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == "Taxonomy" && x.DisplayText == "Categories").FirstOrDefaultAsync();
            if (ci != null)
                _session.Delete(ci);
            ci = await _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == "Taxonomy" && x.DisplayText == "Tags").FirstOrDefaultAsync();
            if (ci != null)
                _session.Delete(ci);
            await _session.SaveChangesAsync();

            return 2;
        }
        public static bool firstPass = true;//for some reason this script is executed twice on recipe execution
        public async Task<int> UpdateFrom2()
        {
            await _contentDefinitionManager.AlterTypeDefinitionAsync("BlogPost", type => type.RemovePart("MarkdownBodyPart")
                .DisplayedAs("Blog Post")
                .Draftable()
                .Versionable()
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("2")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        Pattern = "{{ Model.ContentItem | container | display_text | slugify }}/{{ Model.ContentItem | display_text | slugify }}",
                        ShowHomepageOption = false,
                    })
                )
                .WithPart("BlogPost", part => part
                    .WithPosition("3")
                )
                .WithPart("HtmlBodyPart", part => part
                    .WithPosition("1")
                    .WithEditor("Wysiwyg")
                )
            );


            await _contentDefinitionManager.AlterPartDefinitionAsync("BlogPost", part => part.RemoveField("Category").RemoveField("Tags"));

            await _contentDefinitionManager.AlterPartDefinitionAsync("BlogPost", part => part
                .WithField("Subtitle", field => field
                    .OfType("TextField")
                    .WithDisplayName("Subtitle")
                    .WithPosition("0")
                )
                .WithField("Image", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Banner Image")
                    .WithPosition("1")
                    .WithSettings(new LuceneContentIndexSettings
                    {
                        Included = false,
                        Stored = false,
                    })
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                        AllowAnchors = true,
                    })
                )
                .WithField("Tags", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Tags")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("2")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "45j76cwwz4f4v4hx5zqxfpzvwq",
                    })
                )
                .WithField("Category", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Category")
                    .WithPosition("3")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        TaxonomyContentItemId = "4dgj6ce33vdsbxqz8hw4c4c24d",
                        Unique = true,
                        LeavesOnly = true,
                    })
                )
            );
            if (firstPass)
                await _migrator.ExecuteAsync("tags-cats.json", this);
            firstPass = false;
            return 3;
        }

        public async Task<int> UpdateFrom3()
        {

            await _migrator.ExecuteAsync("localization.recipe.json", this);

            return 4;
        }

        public async Task<int> UpdateFrom4()
        {
            await _contentDefinitionManager.AlterPartDefinitionAsync("GPiece", cfg => cfg
               .WithField("Link",
                   fieldBuilder => fieldBuilder
                       .OfType("TextField")
                       .WithDisplayName("Link")
                       .WithEditor("Url"))
           );
            return 5;
        }

        public async Task<int> UpdateFrom5()
        {

            await _migrator.ExecuteAsync("localizemenu.recipe.json", this);

            return 6;
        }
    }
}