using Members.Persons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public interface IPartService<T>
    {
        IAsyncEnumerable<ValidationResult> ValidateAsync(T part);

        Task InitializingAsync(T part);
        Task PublishedAsync(T instance, PublishContentContext context);
        Task OnUpdatingAsync(T model, IUpdateModel updater, UpdatePartEditorContext context);

        Action<T> GetEditModel(T part, BuildPartEditorContext context);
        Task UpdatedAsync<TPart>(UpdateContentContext context, T instance);
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

    public abstract class PartService<T> : IPartService<T>
    {
        private IHttpContextAccessor _httpCa;

        public PartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpCa = httpContextAccessor;
        }
        
        public bool IsAdmin => AdminAttribute.IsApplied(_httpCa.HttpContext);

    public virtual Task InitializingAsync(T part)
        {
            return Task.CompletedTask;
        }

        public virtual Action<T> GetEditModel(T part, BuildPartEditorContext context)
        {
            return null;
        }

        public virtual Task OnUpdatingAsync(T model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task PublishedAsync(T instance, PublishContentContext context)
        {
            return Task.CompletedTask;
        }

        public virtual IAsyncEnumerable<ValidationResult> ValidateAsync(T part)
        {
            return Validate(part).ToAsyncEnumerable();
        }

        public virtual IEnumerable<ValidationResult> Validate(T part)
        {
            return Array.Empty<ValidationResult>();
        }

        public virtual Task UpdatedAsync<TPart>(UpdateContentContext context, T instance)
        {
            return Task.CompletedTask;
        }
    }
}
