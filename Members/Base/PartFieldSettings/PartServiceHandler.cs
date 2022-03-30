using Members.PartFieldSettings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;

namespace Members.Persons
{
    public class PartServiceHandler<TPart,TService>:ContentPartHandler<TPart>
        where TPart:ContentPart,new()
        where TService:IPartService<TPart>
    {
        private readonly TService _service;

        public PartServiceHandler(
            TService service)
        {
            _service = service;
        }

        public override async Task ValidatingAsync(ValidateContentContext context, TPart part)
        {

            await foreach (var item in _service.ValidateAsync(part))
            {
                context.Fail(item);
            }
        }

        public override async Task InitializingAsync(InitializingContentContext context, TPart instance)
        {
            await _service.InitializingAsync(instance);
            context.ContentItem.Apply(instance);
        }

        public override async Task PublishedAsync(PublishContentContext context, TPart instance)
        {
            await _service.PublishedAsync(instance, context);
        }

        public override async Task UpdatedAsync(UpdateContentContext context, TPart instance)
        {
           await _service.UpdatedAsync(context,instance);
            instance.ContentItem.Apply(instance);
        }
    }
}
