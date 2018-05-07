namespace LittleSteve.Data.Entities
{
    public class TwitchAlertSubscription
    {
        public int Id { get; set; }
        public long DiscordChannelId { get; set; }
        public TwitchStreamer TwitchStreamer { get; set; }
        public string StreamMessage { get; set; }
        public long TwitchStreamerId { get; set; }
        public long MessageId { get; set; }
        public bool ShouldPin { get; set; }
    }
}