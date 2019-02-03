﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class YoutubeSubscription
    {
        public int Id { get; set; }
        public string DiscordChannelId { get; set; }
        public Youtube Youtube { get; set; }
        public string YoutubeId { get; set; }


    }
}