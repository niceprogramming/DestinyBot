using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class Youtube
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long LatestVideoDate { get; set; }
        public ICollection<YoutubeSubscription> YoutubeSubscriptions { get; set; }
        [NotMapped] public string ChannelUrl => $"https://www.youtube.com/channel/{Id}";
    }
}
