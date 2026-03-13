using System.Text.RegularExpressions;
using ChatBot.Models;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;

namespace ChatBot.Services;

public partial class ContentIndexingService
{
    private readonly ISession _session;
    private readonly IContentManager _contentManager;
    private readonly EmbeddingService _embeddingService;
    private readonly VectorStoreService _vectorStoreService;
    private readonly ILogger<ContentIndexingService> _logger;

    private static volatile bool _indexed;
    private static readonly SemaphoreSlim _indexLock = new(1, 1);

    public ContentIndexingService(
        ISession session,
        IContentManager contentManager,
        EmbeddingService embeddingService,
        VectorStoreService vectorStoreService,
        ILogger<ContentIndexingService> logger)
    {
        _session = session;
        _contentManager = contentManager;
        _embeddingService = embeddingService;
        _vectorStoreService = vectorStoreService;
        _logger = logger;
    }

    public bool IsIndexed => _indexed;

    public async Task EnsureIndexedAsync()
    {
        if (_indexed) return;
        await IndexAllContentAsync();
    }

    public async Task IndexAllContentAsync()
    {
        await _indexLock.WaitAsync();
        try
        {
            _logger.LogInformation("Starting content indexing...");

            var contentTypes = new[] { "Page", "Article", "BlogPost", "Blog" };
            var allChunks = new List<ContentChunk>();

            foreach (var contentType in contentTypes)
            {
                var items = await _session.Query<ContentItem, ContentItemIndex>(x => x.ContentType == contentType && x.Published).ListAsync();
                foreach (var item in items)
                {
                    var chunks = ExtractChunks(item);
                    allChunks.AddRange(chunks);
                }
            }

            if (allChunks.Count == 0)
            {
                _logger.LogWarning("No content items found to index");
                _indexed = true;
                return;
            }

            _logger.LogInformation("Extracted {Count} chunks from content items", allChunks.Count);

            const int embeddingBatchSize = 50;
            var upsertItems = new List<(string Id, float[] Vector, Dictionary<string, string> Payload)>();

            for (int i = 0; i < allChunks.Count; i += embeddingBatchSize)
            {
                var batch = allChunks.Skip(i).Take(embeddingBatchSize).ToList();
                var texts = batch.Select(c => $"{c.Title}\n{c.Content}").ToList();
                var embeddings = await _embeddingService.GetEmbeddingsAsync(texts);

                for (int j = 0; j < batch.Count; j++)
                {
                    var chunk = batch[j];
                    upsertItems.Add((
                        chunk.Id,
                        embeddings[j],
                        new Dictionary<string, string>
                        {
                            ["title"] = chunk.Title,
                            ["content"] = chunk.Content,
                            ["url"] = chunk.Url,
                            ["culture"] = chunk.Culture,
                            ["chunk_index"] = allChunks.IndexOf(chunk).ToString()
                        }
                    ));
                }
            }

            await _vectorStoreService.UpsertBatchAsync(upsertItems);

            _indexed = true;
            _logger.LogInformation("Content indexing complete. Indexed {Count} chunks.", allChunks.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Content indexing failed");
            throw;
        }
        finally
        {
            _indexLock.Release();
        }
    }

    private List<ContentChunk> ExtractChunks(ContentItem item)
    {
        var chunks = new List<ContentChunk>();

        var title = item.Content.TitlePart?.Title?.ToString() as string
            ?? item.DisplayText
            ?? "";

        var html = item.Content.HtmlBodyPart?.Html?.ToString() as string ?? "";
        var body = StripHtml(html);

        var url = item.Content.AutoroutePart?.Path?.ToString() as string ?? "";
        var culture = item.Content.LocalizationPart?.Culture?.ToString() as string ?? "hr";

        if (string.IsNullOrWhiteSpace(body) && string.IsNullOrWhiteSpace(title))
            return chunks;

        var textChunks = ChunkText(body, 2000);

        if (textChunks.Count == 0)
        {
            chunks.Add(new ContentChunk
            {
                Id = $"{item.ContentItemId}_0",
                Title = title,
                Content = title,
                Url = url,
                Culture = culture
            });
        }
        else
        {
            for (int i = 0; i < textChunks.Count; i++)
            {
                chunks.Add(new ContentChunk
                {
                    Id = $"{item.ContentItemId}_{i}",
                    Title = title,
                    Content = textChunks[i],
                    Url = url,
                    Culture = culture
                });
            }
        }

        return chunks;
    }

    private static string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return "";
        var text = HtmlTagRegex().Replace(html, " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = WhitespaceRegex().Replace(text, " ").Trim();
        return text;
    }

    private static List<string> ChunkText(string text, int maxChars)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        if (text.Length <= maxChars)
            return [text];

        var chunks = new List<string>();
        var sentences = SentenceBoundaryRegex().Split(text);
        var current = "";

        foreach (var sentence in sentences)
        {
            var trimmed = sentence.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            if (current.Length + trimmed.Length + 1 > maxChars && current.Length > 0)
            {
                chunks.Add(current.Trim());
                current = "";
            }
            current += (current.Length > 0 ? " " : "") + trimmed;
        }

        if (current.Trim().Length > 0)
            chunks.Add(current.Trim());

        return chunks;
    }

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"(?<=[.!?])\s+")]
    private static partial Regex SentenceBoundaryRegex();
}
