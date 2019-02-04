using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchUserResponse
    {
        [JsonProperty("data")] public List<User> Users { get; set; }
    }
}