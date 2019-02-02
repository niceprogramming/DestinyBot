using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class Stream
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("game_id")]
        public int GameId { get; set; }

        [JsonProperty("community_ids")]
        public List<string> CommunityIds { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("viewer_count")]
        public int ViewerCount { get; set; }

        [JsonProperty("started_at")]
        public DateTimeOffset StartedAt { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("tag_ids")]
        public List<string> TagIds { get; set; }
    }
}