using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using Qdrant.Client;

namespace ChatBot;

public class Startup : OrchardCore.Modules.StartupBase
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ChatBotOptions>(_configuration.GetSection(ChatBotOptions.Section));

        var options = _configuration.GetSection(ChatBotOptions.Section).Get<ChatBotOptions>() ?? new ChatBotOptions();

        if (!string.IsNullOrEmpty(options.OpenAiApiKey))
        {
            OpenAIClientOptions? clientOptions = null;
            if (!string.IsNullOrEmpty(options.OpenAiBaseUrl))
            {
                clientOptions = new OpenAIClientOptions
                {
                    Endpoint = new Uri(options.OpenAiBaseUrl)
                };
            }

            var credential = new System.ClientModel.ApiKeyCredential(options.OpenAiApiKey);
            var openAiClient = new OpenAIClient(credential, clientOptions);
            services.AddSingleton(openAiClient.GetChatClient(options.OpenAiModel));
            services.AddSingleton(openAiClient.GetEmbeddingClient(options.EmbeddingModel));
        }
        else
        {
            services.AddSingleton(new ChatClient(model: options.OpenAiModel, credential: new System.ClientModel.ApiKeyCredential("placeholder")));
            services.AddSingleton(new EmbeddingClient(model: options.EmbeddingModel, credential: new System.ClientModel.ApiKeyCredential("placeholder")));
        }

        services.AddSingleton(new QdrantClient(options.QdrantHost, options.QdrantPort));

        services.AddScoped<EmbeddingService>();
        services.AddScoped<VectorStoreService>();
        services.AddScoped<ContentIndexingService>();
        services.AddScoped<ChatService>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
    }
}
