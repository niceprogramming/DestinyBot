using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class YoutubeSubscription
    {
        public string DiscordChannelId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public long LatestVideoDate { get; set; }
        [NotMapped] public string ChannelUrl => $"https://www.youtube.com/channel/{ChannelId}";
    }
}