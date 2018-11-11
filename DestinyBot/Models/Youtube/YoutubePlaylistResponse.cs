using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubePlaylistResponse
    {
        [JsonProperty("items")]
        public List<YoutubeVideo> YoutubeVideos { get; set; }
    }
}
