using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyBot.Data.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public string Name { get; set; }
        [NotMapped] public TimeSpan PlayLength => DateTimeOffset.FromUnixTimeSeconds(EndTime) - DateTimeOffset.FromUnixTimeSeconds(StartTime);
        public TwitchStreamer TwitchStreamer { get; set; }
        public long TwitchStreamerId { get; set; }
    }
}
