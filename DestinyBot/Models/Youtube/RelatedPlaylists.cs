using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class RelatedPlaylists
    {

        [JsonProperty("uploads")]
        public string Uploads { get; set; }

        [JsonProperty("watchHistory")]
        public string WatchHistory { get; set; }

        [JsonProperty("watchLater")]
        public string WatchLater { get; set; }
    }
}