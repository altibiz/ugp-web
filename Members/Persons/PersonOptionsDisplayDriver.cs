using OrchardCore.Contents.ViewModels;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace Members.Persons
    {
        public class PersonOptionsDisplayDriver : DisplayDriver<ContentOptionsViewModel>
        {
            // Maintain the Options prefix for compatability with binding.
            protected override void BuildPrefix(ContentOptionsViewModel model, string htmlFieldPrefix)
            {
                Prefix = "Options";
            }

            public override IDisplayResult Display(ContentOptionsViewModel model)
            {
                return Combine(
                    View("ContentsAdminFilters_Thumbnail__Oib", model).Location("Thumbnail", "Content:10")
                );
            }
        }
    }
