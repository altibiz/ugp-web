using Members.Persons;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public interface IPartService<T>
    {
        IAsyncEnumerable<ValidationResult> ValidateAsync(T part);

        Task InitializingAsync(T part);
        Task PublishedAsync(T instance);
    }

    public static class PartServiceExtensions
    {
        public static void UsePartService<TPart, TService>(this IServiceCollection services)
            where TPart : ContentPart, new()
            where TService : class, IPartService<TPart>
        {
            services.AddScoped<TService, TService>();
            services.AddContentPart<TPart>()
                .AddHandler<PartServiceHandler<TPart, TService>>()
                .UseDisplayDriver<PartServiceDisplayDriver<TPart, TService>>();
        }
    }
}
