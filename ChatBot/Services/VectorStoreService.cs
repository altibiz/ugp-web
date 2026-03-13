using ChatBot.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace ChatBot.Services;

public class VectorStoreService
{
    private readonly QdrantClient _client;
    private readonly ChatBotOptions _options;
    private readonly ILogger<VectorStoreService> _logger;
    private bool _collectionEnsured;
    private readonly SemaphoreSlim _ensureLock = new(1, 1);

    public VectorStoreService(
        QdrantClient client,
        IOptions<ChatBotOptions> options,
        ILogger<VectorStoreService> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task EnsureCollectionAsync()
    {
        if (_collectionEnsured) return;

        await _ensureLock.WaitAsync();
        try
        {
            if (_collectionEnsured) return;

            var exists = await _client.CollectionExistsAsync(_options.CollectionName);
            if (!exists)
            {
                await _client.CreateCollectionAsync(
                    _options.CollectionName,
                    new VectorParams { Size = 1536, Distance = Distance.Cosine });
                _logger.LogInformation("Created Qdrant collection {Collection}", _options.CollectionName);
            }

            _collectionEnsured = true;
        }
        finally
        {
            _ensureLock.Release();
        }
    }

    public async Task UpsertAsync(string id, float[] vector, Dictionary<string, string> payload)
    {
        await EnsureCollectionAsync();

        var point = new PointStruct
        {
            Id = new PointId { Uuid = GenerateDeterministicGuid(id).ToString() },
            Vectors = vector,
        };

        foreach (var kvp in payload)
        {
            point.Payload.Add(kvp.Key, kvp.Value);
        }

        await _client.UpsertAsync(_options.CollectionName, [point]);
    }

    public async Task UpsertBatchAsync(IReadOnlyList<(string Id, float[] Vector, Dictionary<string, string> Payload)> items)
    {
        await EnsureCollectionAsync();

        var points = new List<PointStruct>(items.Count);
        foreach (var item in items)
        {
            var point = new PointStruct
            {
                Id = new PointId { Uuid = GenerateDeterministicGuid(item.Id).ToString() },
                Vectors = item.Vector,
            };
            foreach (var kvp in item.Payload)
            {
                point.Payload.Add(kvp.Key, kvp.Value);
            }
            points.Add(point);
        }

        const int batchSize = 100;
        for (int i = 0; i < points.Count; i += batchSize)
        {
            var batch = points.Skip(i).Take(batchSize).ToList();
            await _client.UpsertAsync(_options.CollectionName, batch);
        }
    }

    public async Task<List<(string Id, float Score, Dictionary<string, string> Payload)>> SearchAsync(
        float[] queryVector, int limit, string? cultureFilter = null)
    {
        await EnsureCollectionAsync();

        Filter? filter = null;
        if (!string.IsNullOrEmpty(cultureFilter))
        {
            filter = new Filter();
            filter.Must.Add(new Condition
            {
                Field = new FieldCondition
                {
                    Key = "culture",
                    Match = new Match { Keyword = cultureFilter }
                }
            });
        }

        var results = await _client.SearchAsync(
            _options.CollectionName,
            queryVector,
            filter: filter,
            limit: (ulong)limit,
            payloadSelector: true);

        return results.Select(r => (
            Id: r.Id.Uuid,
            Score: r.Score,
            Payload: r.Payload.ToDictionary(
                p => p.Key,
                p => p.Value.StringValue ?? "")
        )).ToList();
    }

    private static Guid GenerateDeterministicGuid(string input)
    {
        var hash = System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }
}
