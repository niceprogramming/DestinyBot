using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeChannelContentDetails
    {
        [JsonProperty("relatedPlaylists")]
        public RelatedPlaylists RelatedPlaylists { get; set; }
    }
}