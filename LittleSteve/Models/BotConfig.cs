using Newtonsoft.Json;

namespace LittleSteve.Models
{
    public class BotConfig
    {
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessTokenSecret { get; set; }
        public string ConnectionString { get; set; }
        public string YoutubeKey { get; set; }
        public string TwitchClientId { get; set; }
        public string DiscordToken { get; set; }
        public string ImgurClientId { get; set; }
        public string DestinyCalendar { get; set; }
        public string AslanAlbumId { get; set; }
        public string RMSAlbumId { get; set; }
        public string Prefix { get; set; }
        public int ThrottleLength { get; set; }
    }
}