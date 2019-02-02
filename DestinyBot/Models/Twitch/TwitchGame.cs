using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchGame
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("box_art_url")]
        public string BoxArtUrl { get; set; }
    }
}
