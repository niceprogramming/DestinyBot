namespace DestinyBot.Data.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public long DateCreated { get; set; }
        public long TimeToRemind { get; set; }
        public string Message { get; set; }
        public long ChannelId { get; set; }
        public long GuildId { get; set; }
        public long UserId { get; set; }
    }
}