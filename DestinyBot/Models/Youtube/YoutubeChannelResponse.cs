using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Youtube
{
    public class YoutubeChannelResponse
    {
        [JsonProperty("items")]
        public List<YoutubeChannel> YoutubeChannels { get; set; }
    }
}
