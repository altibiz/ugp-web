using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatBot.Controllers;

[Route("api/chat")]
[ApiController]
[IgnoreAntiforgeryToken]
public class ChatApiController : Controller
{
    private readonly ChatService _chatService;
    private readonly ILogger<ChatApiController> _logger;

    public ChatApiController(ChatService chatService, ILogger<ChatApiController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpPost]
    public async Task Post()
    {
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";

        try
        {
            var request = await JsonSerializer.DeserializeAsync<ChatRequest>(
                Request.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request?.Messages == null || request.Messages.Count == 0)
            {
                await WriteSseChunk("I didn't receive a message. Please try again.");
                return;
            }

            var culture = DetectCulture();

            await foreach (var chunk in _chatService.StreamResponseAsync(
                request.Messages, culture, HttpContext.RequestAborted))
            {
                await WriteSseChunk(chunk);
            }
        }
        catch (OperationCanceledException)
        {
            // Client disconnected
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chat API error");
            await WriteSseChunk("Sorry, an error occurred. Please try again later.");
        }
    }

    private async Task WriteSseChunk(string text)
    {
        var json = JsonSerializer.Serialize(new { text });
        await Response.WriteAsync($"data: {json}\n\n");
        await Response.Body.FlushAsync();
    }

    private string DetectCulture()
    {
        var referer = Request.Headers.Referer.ToString();
        if (string.IsNullOrEmpty(referer))
            return "hr";

        try
        {
            var uri = new Uri(referer);
            var path = uri.AbsolutePath.TrimStart('/');

            if (path.StartsWith("en/", StringComparison.OrdinalIgnoreCase) || path.Equals("en", StringComparison.OrdinalIgnoreCase))
                return "en";
            if (path.StartsWith("de/", StringComparison.OrdinalIgnoreCase) || path.Equals("de", StringComparison.OrdinalIgnoreCase))
                return "de";
        }
        catch
        {
            // Invalid referer URL
        }

        return "hr";
    }
}
