using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Models;
using DestinyBot.Models.Youtube;
using DestinyBot.Services;
using Discord.WebSocket;
using FluentScheduler;

namespace DestinyBot.Jobs
{
    public class YoutubeJob : IJob
    {
        private readonly string _id;
        private readonly string _name;
        private readonly DiscordSocketClient _client;
        private readonly DestinyBotContext _db;
        

        private readonly YoutubeService _youtube;
        private const string YoutubeUrlTemplate = "https://www.youtube.com/watch?v={0}";

        public YoutubeJob(string name,YoutubeService youtube, DiscordSocketClient client, DestinyBotContext db)
        {
            _name = name;
            _client = client;
            _db = db;
            

            _youtube = youtube;
        }
        public void Execute()
        {
            using (_db)
            {
                var subs = _db.YoutubeSubscriptions.Where(x => x.ChannelName == _name);
                var video = _youtube.GetLatestVideoAsync(_name).GetAwaiter().GetResult() ?? new YoutubeVideo();
                if (subs.FirstOrDefault()?.LatestVideoDate >= video.Snippet.PublishedAt.ToUnixTimeSeconds())
                {
                    return;
                }

                foreach (var sub in subs)
                {
                    var channel = _client.GetChannel(Convert.ToUInt64(sub.DiscordChannelId)) as SocketTextChannel;
                    channel?.SendMessageAsync(string.Format(YoutubeUrlTemplate, video.Snippet.ResourceId.VideoId));
                    sub.LatestVideoDate = video.Snippet.PublishedAt.ToUnixTimeSeconds();
                }

                _db.SaveChanges();
            }
        }
    }
}
