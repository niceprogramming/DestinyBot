using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchStreamResponse
    {
        [JsonProperty("data")] public List<Stream> Streams { get; set; }
    }
}