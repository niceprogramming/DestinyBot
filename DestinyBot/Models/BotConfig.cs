namespace DestinyBot.Models
{
    public class BotConfig
    {
        public string ConnectionString { get; set; }
        public string YoutubeKey { get; set; }
        public string TwitchClientId { get; set; }
        public string TwitchClientSecret { get; set; }
        public string DiscordToken { get; set; }
        public string ImgurClientId { get; set; }
        public string ImgurAlbumId { get; set; }
        public string Prefix { get; set; }
        public int ThrottleLength { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessSecret { get; set; }
    }
}