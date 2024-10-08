﻿using Etch.OrchardCore.Fields.ResponsiveMedia.Models;
using OrchardCore.Media;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Etch.OrchardCore.Fields.ResponsiveMedia.Utils
{
    public static class ResponsiveMediaUtils
    {
        public static IList<ResponsiveMediaItem> ParseMedia(IMediaFileStore mediaFileStore, string data)
        {
            var media = new List<ResponsiveMediaItem>();

            if (!string.IsNullOrWhiteSpace(data))
            {
                media = data.StartsWith("[") ? JsonSerializer.Deserialize<List<ResponsiveMediaItem>>(data) : new List<ResponsiveMediaItem> { JsonSerializer.Deserialize<ResponsiveMediaItem>(data) };
            }

            foreach (var mediaItem in media)
            {
                if (mediaItem.Sources == null)
                {
                    continue;
                }

                foreach (var source in mediaItem.Sources)
                {
                    source.Name = Path.GetFileName(source.Path);
                    source.Url = mediaFileStore.MapPathToPublicUrl(source.Path);
                }
            }

            return media;
        }
    }
}
