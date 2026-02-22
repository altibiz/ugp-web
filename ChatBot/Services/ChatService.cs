using System.Runtime.CompilerServices;
using ChatBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace ChatBot.Services;

public class ChatService
{
    private readonly ChatClient _chatClient;
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStoreService;
    private readonly ContentIndexingService _contentIndexingService;
    private readonly ChatBotOptions _options;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        ChatClient chatClient,
        EmbeddingService embeddingService,
        VectorStoreService vectorStoreService,
        ContentIndexingService contentIndexingService,
        IOptions<ChatBotOptions> options,
        ILogger<ChatService> logger)
    {
        _chatClient = chatClient;
        _embeddingService = embeddingService;
        _vectorStoreService = vectorStoreService;
        _contentIndexingService = contentIndexingService;
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> StreamResponseAsync(
        List<Models.ChatMessage> messages,
        string culture,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await _contentIndexingService.EnsureIndexedAsync();

        var lastUserMessage = messages.LastOrDefault(m =>
            m.Role.Equals("user", StringComparison.OrdinalIgnoreCase));
        if (lastUserMessage == null)
            yield break;

        var queryVector = await _embeddingService.GetEmbeddingAsync(lastUserMessage.Text);
        var searchResults = await _vectorStoreService.SearchAsync(
            queryVector, _options.MaxRetrievedChunks, culture);

        var language = culture switch
        {
            "en" => "English",
            "de" => "German",
            _ => "Croatian"
        };

        var contextParts = searchResults.Select(r =>
        {
            r.Payload.TryGetValue("title", out var title);
            r.Payload.TryGetValue("content", out var content);
            return $"{title}:\n{content}";
        });

        var systemPrompt = $"""
            You are a helpful assistant for the Glas Poduzetnika (Voice of Entrepreneurs) website.
            Answer questions based on the following context from the website. If you cannot find the answer in the context, say so honestly.
            Respond in {language} language.

            Context:
            ---
            {string.Join("\n---\n", contextParts)}
            """;

        var chatMessages = new List<OpenAI.Chat.ChatMessage>
        {
            new SystemChatMessage(systemPrompt)
        };

        var recentMessages = messages
            .TakeLast(_options.MaxConversationMessages)
            .ToList();

        foreach (var msg in recentMessages)
        {
            if (msg.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                chatMessages.Add(new UserChatMessage(msg.Text));
            else if (msg.Role.Equals("ai", StringComparison.OrdinalIgnoreCase))
                chatMessages.Add(new AssistantChatMessage(msg.Text));
        }

        var streamingUpdates = _chatClient.CompleteChatStreamingAsync(
            chatMessages, cancellationToken: cancellationToken);

        await foreach (var update in streamingUpdates.WithCancellation(cancellationToken))
        {
            foreach (var part in update.ContentUpdate)
            {
                if (!string.IsNullOrEmpty(part.Text))
                    yield return part.Text;
            }
        }
    }
}
