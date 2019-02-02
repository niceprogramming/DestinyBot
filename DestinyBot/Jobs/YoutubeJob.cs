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
using Microsoft.EntityFrameworkCore;

namespace DestinyBot.Jobs
{
    public class YoutubeJob : IJob
    {
        
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
                var channel = _db.Channels.Include(x => x.YoutubeSubscriptions).FirstOrDefault(x => x.Name == _name);
                var video = _youtube.GetLatestVideoAsync(_name).GetAwaiter().GetResult() ?? new YoutubeVideo();
                if (channel?.LatestVideoDate >= video.Snippet.PublishedAt.ToUnixTimeSeconds())
                {
                    return;
                }
                channel.LatestVideoDate = video.Snippet.PublishedAt.ToUnixTimeSeconds();
                foreach (var sub in channel.YoutubeSubscriptions)
                {
                    var textChannel = _client.GetChannel(Convert.ToUInt64(sub.DiscordChannelId)) as SocketTextChannel;
                    textChannel?.SendMessageAsync(string.Format(YoutubeUrlTemplate, video.Snippet.ResourceId.VideoId));
                }

                _db.SaveChanges();
            }
        }
    }
}
