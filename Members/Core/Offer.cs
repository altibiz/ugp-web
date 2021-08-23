using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Taxonomies.Fields;
using OrchardCore.Taxonomies.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.Core
{
    
public class Offer : ContentPart
{
    public TextField ShortDescription { get; set; }
    public TextField PersonName { get; set; }
    public TextField DisplayText { get; set; }
    public TextField ContactPerson { get; set; }
    public TextField Email { get; set; }
    public TextField LongDescription { get; set; }
    public LinkField YoutubeVideo { get; set; }
    public TextField Address { get; set; }
    public TextField Phone { get; set; }
    public LinkField Web { get; set; }
    public LinkField Instagram { get; set; }
    public LinkField Facebook { get; set; }
    public LinkField LinkedIn { get; set; }
    public TaxonomyField category { get; set; }
}



    public static class OfferMigration
    {
        public static void MigrateOffer(this IContentDefinitionManager _contentDefinitionManager)
        {
            _contentDefinitionManager.AlterTypeDefinition("Offer", type => type
                .DisplayedAs("Ponuda")
                .Creatable()
                .Listable()
                .Securable()
                .WithPart("Offer", part => part
                    .WithPosition("0")
                )
            );

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
                .WithField("DisplayText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Nalov")
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
                .WithField("YoutubeVideo", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Youtube video")
                    .WithPosition("7")
                    .WithSettings(new LinkFieldSettings
                    {
                        LinkTextMode = LinkTextMode.Url,
                    })
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
                .WithField("category", field => field
                    .OfType("TaxonomyField")
                    .WithDisplayName("Kategorija")
                    .WithEditor("Tags")
                    .WithDisplayMode("Tags")
                    .WithPosition("4")
                    .WithSettings(new TaxonomyFieldSettings
                    {
                        Required = true,
                        TaxonomyContentItemId = "4a6d7mtpab04yt9yedrsardz4r",
                    })
                )
            );

        }
    }
}
