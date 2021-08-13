using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Settings;

namespace OrchardCore.Themes.UgpTheme
{
    public class Migrations : DataMigration {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager) {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create() {
            _contentDefinitionManager.AlterPartDefinition("GPiece", cfg => cfg
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
                        .WithSettings(new MediaFieldSettings { Required = true, Multiple = false}))
                .WithField("ImageClass",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Image Class"))
                .WithField("ImageAltText",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Image Alt Text"))
            );

            _contentDefinitionManager.AlterTypeDefinition("GPiece", type => type
                .WithPart("GPiece"));

            _contentDefinitionManager.AlterPartDefinition("Gallery", cfg => cfg
                .WithDescription("Contains the fields for the current type")
                .WithField("DisplayType",
                    fieldBuilder => fieldBuilder
                        .OfType("TextField")
                        .WithDisplayName("Display Type"))
            );

            _contentDefinitionManager.AlterTypeDefinition("Gallery", type => type
                .WithPart("TitlePart")
                .WithPart("Gallery")
                .WithPart("GPieces", "BagPart", cfg => cfg
                    .WithDisplayName("GPieces")
                    .WithDescription("GPieces to display in the carousel.")
                    .WithSettings(new BagPartSettings { ContainedContentTypes = new[] { "GPiece" }, DisplayType = "Detail" }))
                .Stereotype("Widget"));

            return 1;
        }
    }
}