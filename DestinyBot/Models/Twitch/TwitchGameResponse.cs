using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchGameResponse
    {
        [JsonProperty("data")] public List<TwitchGame> Games { get; set; }
    }
}