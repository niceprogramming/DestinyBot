using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models
{
    public class Image
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("title")] public object Title { get; set; }

        [JsonProperty("description")] public object Description { get; set; }

        [JsonProperty("datetime")] public long Datetime { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("animated")] public bool Animated { get; set; }

        [JsonProperty("width")] public long Width { get; set; }

        [JsonProperty("height")] public long Height { get; set; }

        [JsonProperty("size")] public long Size { get; set; }

        [JsonProperty("views")] public long Views { get; set; }

        [JsonProperty("bandwidth")] public long Bandwidth { get; set; }

        [JsonProperty("vote")] public object Vote { get; set; }

        [JsonProperty("favorite")] public bool Favorite { get; set; }

        [JsonProperty("nsfw")] public object Nsfw { get; set; }

        [JsonProperty("section")] public object Section { get; set; }

        [JsonProperty("account_url")] public object AccountUrl { get; set; }

        [JsonProperty("account_id")] public object AccountId { get; set; }

        [JsonProperty("is_ad")] public bool IsAd { get; set; }

        [JsonProperty("in_most_viral")] public bool InMostViral { get; set; }

        [JsonProperty("has_sound")] public bool HasSound { get; set; }

        [JsonProperty("tags")] public List<object> Tags { get; set; }

        [JsonProperty("ad_type")] public long AdType { get; set; }

        [JsonProperty("ad_url")] public string AdUrl { get; set; }

        [JsonProperty("in_gallery")] public bool InGallery { get; set; }

        [JsonProperty("link")] public string Link { get; set; }
    }
}