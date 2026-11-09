using System.ClientModel;
using ChatBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;

namespace ChatBot.Services;

public class EmbeddingService
{
    private readonly EmbeddingClient _client;
    private readonly ILogger<EmbeddingService> _logger;

    public EmbeddingService(EmbeddingClient client, ILogger<EmbeddingService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        ClientResult<OpenAIEmbedding> result = await _client.GenerateEmbeddingAsync(text);
        return result.Value.ToFloats().ToArray();
    }

    public async Task<float[][]> GetEmbeddingsAsync(IEnumerable<string> texts)
    {
        var textList = texts.ToList();
        if (textList.Count == 0)
            return [];

        ClientResult<OpenAIEmbeddingCollection> result = await _client.GenerateEmbeddingsAsync(textList);
        return result.Value.Select(e => e.ToFloats().ToArray()).ToArray();
    }
}
