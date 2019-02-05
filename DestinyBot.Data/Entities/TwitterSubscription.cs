using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class TwitterSubscription
    {
        public int Id { get; set; }
        public long DiscordChannelId { get; set; }
        public TwitterUser User { get; set; }
        public long TwitterUserId { get; set; }
    }
}
