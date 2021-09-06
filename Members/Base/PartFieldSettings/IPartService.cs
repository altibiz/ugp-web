using Members.Persons;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Members.PartFieldSettings
{
    public interface IPartService<T>
    {
        IAsyncEnumerable<ValidationResult> ValidateAsync(T part);

        Task InitializingAsync(T part);
        Task PublishedAsync(T instance, PublishContentContext context);
        void OnUpdatingAsync(T model, IUpdateModel updater, UpdatePartEditorContext context);

        Action<T> GetEditModel(T part, BuildPartEditorContext context);
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

        public virtual Task InitializingAsync(T part)
        {
            return Task.CompletedTask;
        }

        public virtual Action<T> GetEditModel(T part, BuildPartEditorContext context)
        {
            return null;
        }

        public virtual void OnUpdatingAsync(T model, IUpdateModel updater, UpdatePartEditorContext context)
        {

        }

        public virtual Task PublishedAsync(T instance, PublishContentContext context)
        {
            return Task.CompletedTask;
        }

        public virtual async IAsyncEnumerable<ValidationResult> ValidateAsync(T part)
        {
            yield break;
        }
    }
}
