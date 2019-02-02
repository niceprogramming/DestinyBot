using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchGameResponse
    {
        [JsonProperty("data")]
        public List<TwitchGame> Games { get; set; }
    }
}
