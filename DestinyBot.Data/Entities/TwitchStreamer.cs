using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DestinyBot.Data.Entities
{
    public class TwitchStreamer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long SteamStartTime { get; set; }
        public long StreamEndTime { get; set; }

        [NotMapped]
        public TimeSpan StreamLength => DateTimeOffset.FromUnixTimeSeconds(StreamEndTime) -
                                        DateTimeOffset.FromUnixTimeSeconds(SteamStartTime);

        public ICollection<Game> Games { get; set; }
        public ICollection<TwitchSubscription> TwitchSubscriptions { get; set; }
    }
}