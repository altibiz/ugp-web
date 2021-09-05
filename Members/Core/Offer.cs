using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Settings;
using OrchardCore.Title.Models;

namespace Members.Core
{

    public class Offer : ContentPart
{
        public TextField ShortDescription { get; set; }
        public TextField PersonName { get; set; }
        public TextField Title { get; set; }
        public TextField ContactPerson { get; set; }
        public TextField Email { get; set; }
        public TextField LongDescription { get; set; }
        public TextField Address { get; set; }
        public TextField Phone { get; set; }
        public LinkField Web { get; set; }
        public LinkField Instagram { get; set; }
        public LinkField Facebook { get; set; }
        public LinkField LinkedIn { get; set; }
        public TaxonomyField Category { get; set; }
        public TextField YoutubeVideoId { get; set; }
        public MediaField FeaturedImage { get; set; }
        public ContentPickerField Company { get; set; }
    }



    public static class OfferMigration
    {
        public static void MigrateOffer(this IContentDefinitionManager _contentDefinitionManager)
        {
            #region OfferType
            _contentDefinitionManager.AlterTypeDefinition("Offer", type => type
                .DisplayedAs("Ponuda")
                .Creatable()
                .Listable()
                .Securable()
                .WithPart("Offer", part => part
                    .WithPosition("0")
                )
                                
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                    .WithSettings(new TitlePartSettings
                    {
                        Options = TitlePartOptions.GeneratedDisabled,
                        Pattern = "{{ ContentItem.Content.Offer.Title.Text }}",
                    })
                )
            );
            #endregion
            #region OfferPart
            _contentDefinitionManager.AlterPartDefinition("Offer", part => part
                .WithField("ShortDescription", field => field
                    .OfType("TextField")
                    .WithDisplayName("Kratki opis")
                    .WithPosition("3")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("PersonName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Naziv pravne ili fizičke osobe")
                    .WithPosition("5")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Title", field => field
                    .OfType("TextField")
                    .WithDisplayName("Naslov")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("ContactPerson", field => field
                    .OfType("TextField")
                    .WithDisplayName("Kontakt osoba")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("Email", field => field
                    .OfType("TextField")
                    .WithDisplayName("Email")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Required = true,
                    })
                )
                .WithField("LongDescription", field => field
                    .OfType("TextField")
                    .WithDisplayName("Širi opis i dodatne specifikacije")
                    .WithPosition("6")
                )
                .WithField("Address", field => field
                    .OfType("TextField")
                    .WithDisplayName("Adresa")
                    .WithPosition("8")
                )
                .WithField("Phone", field => field
                    .OfType("TextField")
                    .WithDisplayName("Telefon")
                    .WithPosition("9")
                )
                .WithField("Web", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Web")
                    .WithPosition("10")
                    .WithSettings(new LinkFieldSettings
                    {
                        LinkTextMode = LinkTextMode.Url,
                    })
                )
                .WithField("Instagram", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Instagram")
                    .WithPosition("11")
                    .WithSettings(new LinkFieldSettings
                    {
                        LinkTextMode = LinkTextMode.Url,
                    })
                )
                .WithField("Facebook", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Facebook")
                    .WithPosition("12")
                    .WithSettings(new LinkFieldSettings
                    {
                        LinkTextMode = LinkTextMode.Url,
                    })
                )
                .WithField("LinkedIn", field => field
                    .OfType("LinkField")
                    .WithDisplayName("LinkedIn")
                    .WithPosition("13")
                    .WithSettings(new LinkFieldSettings
                    {
                        LinkTextMode = LinkTextMode.Url,
                    })
                )
                .WithField("Category", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Kategorija")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("4")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = "4a6d7mtpab04yt9yedrsardz4r",
                        Unique = true,
                    })
                    .WithSettings(new TaxonomyFieldTagsEditorSettings
                    {
                        Open = false,
                    })
                )
                .WithField("YoutubeVideoId", field => field
                    .OfType("TextField")
                    .WithDisplayName("Youtube video ID")
                    .WithPosition("7")
                )
                .WithField("FeaturedImage", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Fotografija")
                    .WithEditor("Attached")
                    .WithSettings(new MediaFieldSettings
                    {
                        Multiple = false,
                        AllowMediaText = false,
                    })
                )
                .WithField("Company", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Company")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Company" },
                    })
                )
            );
            #endregion
        }
    }
}
