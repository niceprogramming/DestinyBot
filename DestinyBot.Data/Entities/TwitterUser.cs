using System.Collections.Generic;

namespace DestinyBot.Data.Entities
{
    public class TwitterUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public long LastTweetId { get; set; }
        public ICollection<TwitterSubscription> TwitterSubscriptions { get; set; }
    }
}