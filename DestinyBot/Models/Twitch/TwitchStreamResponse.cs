﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DestinyBot.Models.Twitch
{
    public class TwitchStreamResponse
    {
        [JsonProperty("data")]
        public List<Stream> Streams { get; set; }
    }
}