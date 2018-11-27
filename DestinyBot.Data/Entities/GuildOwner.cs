using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class GuildOwner
    {
        public string GuildId { get; set; }
        public string ChannelHub { get; set; }
        public string Snowflake { get; set; }
        public string YoutubeId { get; set; }
        public long TwitterId { get; set; }
        public long TwitchId { get; set; }
    }
}
