using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeThumbnails
    {

        [JsonProperty("default")]
        public YoutubeThumbnail Default { get; set; }

        [JsonProperty("medium")]
        public YoutubeThumbnail Medium { get; set; }

        [JsonProperty("high")]
        public YoutubeThumbnail High { get; set; }

        [JsonProperty("standard")]
        public YoutubeThumbnail Standard { get; set; }

        [JsonProperty("maxres")]
        public YoutubeThumbnail Maxres { get; set; }
    }
}