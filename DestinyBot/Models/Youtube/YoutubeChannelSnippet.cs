using System;
using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeChannelSnippet
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("customUrl")] public string CustomUrl { get; set; }

        [JsonProperty("publishedAt")] public DateTime PublishedAt { get; set; }

        [JsonProperty("thumbnails")] public YoutubeThumbnail Thumbnails { get; set; }
    }
}