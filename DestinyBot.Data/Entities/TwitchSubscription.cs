namespace DestinyBot.Data.Entities
{
    public class TwitchSubscription
    {
        public int Id { get; set; }
        public string DiscordChannelId { get; set; }
        public TwitchStreamer TwitchStreamer { get; set; }
        public long TwitchStreamerId { get; set; }
        public string AlternateLink { get; set; }
        public bool Message { get; set; }
        public long MessageId { get; set; }
        public bool ShouldPin { get; set; }
    }
}