using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeThumbnail
    {
        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("width")] public int Width { get; set; }

        [JsonProperty("height")] public int Height { get; set; }
    }
}