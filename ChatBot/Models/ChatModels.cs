namespace ChatBot.Models;

public class ChatMessage
{
    public string Role { get; set; } = "";
    public string Text { get; set; } = "";
}

public class ChatRequest
{
    public List<ChatMessage> Messages { get; set; } = new();
}

public class ContentChunk
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public string Url { get; set; } = "";
    public string Culture { get; set; } = "";
}

public class ChatBotOptions
{
    public const string Section = "ChatBot";

    public string OpenAiApiKey { get; set; } = "";
    public string OpenAiModel { get; set; } = "gpt-4o-mini";
    public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    public string QdrantHost { get; set; } = "localhost";
    public int QdrantPort { get; set; } = 6334;
    public string CollectionName { get; set; } = "ugp_content";
    public int MaxRetrievedChunks { get; set; } = 5;
    public int MaxConversationMessages { get; set; } = 10;
}
