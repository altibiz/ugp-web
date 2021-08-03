using Members.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Members.Indexes
{
    public class PersonPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Oib { get; set; }
        public string LegalName { get; set; }
    }

    public class PersonPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<PersonPartIndex>()
                .Map(contentItem =>
                {
                    var pp = contentItem.As<PersonPart>();
                    if (pp == null) return null;
                    return new PersonPartIndex
                    {
                        ContentItemId = contentItem.ContentItemId,
                        Oib = pp.Oib.Text,
                        LegalName = pp.LegalName
                    };
                });
        }
    }
}
