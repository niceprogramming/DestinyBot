using System;
using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeVideoSnippet
    {
        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("thumbnails")]
        public YoutubeThumbnails YoutubeThumbnails { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }
    }

}
