using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Models.Twitch;
using DestinyBot.Services;
using Discord;
using Discord.WebSocket;
using FluentScheduler;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Game = DestinyBot.Data.Entities.Game;
using TimeUnit = Humanizer.Localisation.TimeUnit;

namespace DestinyBot.Jobs
{
    public class TwitchJob : IJob
    {
        private readonly long _channelId;
        private readonly DiscordSocketClient _client;
        private readonly DestinyBotContext _db;
        private readonly TwitchService _twitchService;

        public TwitchJob(long channelId, TwitchService twitchService, DestinyBotContext db,
            DiscordSocketClient client)
        {
            _channelId = channelId;
            _twitchService = twitchService;
            _db = db;
            _client = client;
        }

        public void Execute()
        {
            using (_db)
            {
                var streamer = _db.TwitchStreamers.Include(x => x.TwitchSubscriptions)
                    .Include(x => x.Games)
                    .FirstOrDefault(x => x.Id == _channelId);

                if (streamer is null) return;

                var isStreaming = _twitchService.IsStreamLiveAsync(streamer.Name).GetAwaiter().GetResult();
                
                //Twitch api is a dumpsterfire and says the stream is on even after it stops for example it will go null -> null -> On -> null -> null.
                //we wait for 3 minutes after the last stream to make sure the streamer in actually on or offline
                //this is arbitrary and seems like it just works
                if (DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(streamer.StreamEndTime) < TimeSpan.FromMinutes(3))
                {
                    return;
                }

                //stream has ended and we are waiting for startup again
                if (!isStreaming && streamer.StreamLength >= TimeSpan.Zero) return;
                // stream started
                if (isStreaming && streamer.StreamLength >= TimeSpan.Zero)
                {
                    var stream = _twitchService.GetStreamAsync(streamer.Name).GetAwaiter().GetResult();

                    streamer.SteamStartTime = stream.StartedAt.ToUnixTimeSeconds();
                    var game = _twitchService.GetGame(stream.GameId).GetAwaiter().GetResult();
                    streamer.Games.Add(new Game
                    {
                        StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        Name = string.IsNullOrWhiteSpace(game.Name) ? "No Game" : game.Name
                    });
                    var logo = _twitchService.GetUserAsync(streamer.Name).GetAwaiter().GetResult()?.ProfileImageUrl;


                    foreach (var subscription in streamer.TwitchSubscriptions)
                    {
                        var messageId = CreateTwitchMessage(streamer, stream, subscription, logo).GetAwaiter()
                            .GetResult();
                        subscription.MessageId = messageId;
                    }

                    _db.SaveChanges();
                    return;
                }

                // we update the message embed for the already started stream
                if (isStreaming && streamer.StreamLength <= TimeSpan.Zero)
                {
                    var stream = _twitchService.GetStreamAsync(streamer.Name).GetAwaiter().GetResult();
                    if (stream is null) return;
                    var game = _twitchService.GetGame(stream.GameId).GetAwaiter().GetResult();

                    var oldGame = streamer.Games.LastOrDefault();
                    var currentGame = game.Name ?? "No Game";
                    if (oldGame != null && oldGame.Name != currentGame)
                    {
                        oldGame.EndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        streamer.Games.Add(new Game
                        {
                            StartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                            Name = currentGame
                        });
                    }

                    var logo = _twitchService.GetUserAsync(streamer.Name).GetAwaiter().GetResult()?.ProfileImageUrl;
                    foreach (var subscription in streamer.TwitchSubscriptions)
                    {
                        if (subscription.MessageId == 0)
                        {
                            var messageId = CreateTwitchMessage(streamer, stream, subscription, logo).GetAwaiter()
                                .GetResult();
                            subscription.MessageId = messageId;
                        }

                        var channel =
                            _client.GetChannel(Convert.ToUInt64(subscription.DiscordChannelId)) as ITextChannel;
                        var message =
                            channel.GetMessageAsync((ulong) subscription.MessageId).GetAwaiter()
                                .GetResult() as IUserMessage;
                        if (message is null)
                        {
                            Log.Information("Message was not found");
                            return;
                        }

                        message.ModifyAsync(x => x.Embed = CreateTwitchEmbed(streamer,subscription, stream, logo)).GetAwaiter()
                            .GetResult();
                    }
                }

                //stream ended
                if (!isStreaming && streamer.StreamLength <= TimeSpan.Zero)
                {
                    streamer.Games.Last().EndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    streamer.StreamEndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    
                    var user = _twitchService.GetUserAsync(streamer.Name).GetAwaiter().GetResult();

                    var description = new StringBuilder();
                    description.AppendLine(
                        $"**Started at:** {DateTimeOffset.FromUnixTimeSeconds(streamer.SteamStartTime):g} UTC");
                    description.AppendLine(
                        $"__**Ended at:** {DateTimeOffset.FromUnixTimeSeconds(streamer.StreamEndTime):g} UTC__");

                    description.AppendLine(
                        $"**Total Time:** {streamer.StreamLength.Humanize(2, maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Minute, collectionSeparator: " ")}");


                    var embed = new EmbedBuilder()
                        .WithAuthor($"{streamer.Name} was live", url: $"https://twitch.tv/{streamer.Name}")
                        .WithThumbnailUrl(user.ProfileImageUrl)
                        .WithDescription(description.ToString())
                        .AddField("Games Played",
                            string.Join("\n",
                                streamer.Games
                                    .Where(x => x.StartTime >= streamer.SteamStartTime &&
                                                x.EndTime <= streamer.StreamEndTime).Select(x =>
                                        $"**{x.Name}:** Played for {x.PlayLength.Humanize(2, maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Minute, collectionSeparator: " ")}")))
                        .Build();

                    foreach (var subscription in streamer.TwitchSubscriptions)
                    {
                        var channel =
                            _client.GetChannel(Convert.ToUInt64(subscription.DiscordChannelId)) as ITextChannel;
                        var message =
                            channel.GetMessageAsync((ulong) subscription.MessageId).GetAwaiter()
                                .GetResult() as IUserMessage;

                        
                        if (message is null)
                        {
                            Log.Information("Message was not found");
                            continue;
                        }

                        message.ModifyAsync(x => x.Embed = embed).GetAwaiter().GetResult();
                    }
                }

                _db.SaveChanges();
            }
        }

        private async Task<long> CreateTwitchMessage(
            TwitchStreamer streamer,
            Stream stream,
            TwitchSubscription subscription,
            string logoUrl)
        {
            var channel = _client.GetChannel(Convert.ToUInt64(subscription.DiscordChannelId)) as ITextChannel;

            var message =
                await channel.SendMessageAsync(string.Empty, embed: CreateTwitchEmbed(streamer, subscription, stream, logoUrl));
            return (long) message.Id;
        }

        private Embed CreateTwitchEmbed(TwitchStreamer streamer,TwitchSubscription subscription, Stream stream, string logoUrl)
        {
            var timeLive = DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(streamer.SteamStartTime);
            var game = _twitchService.GetGame(stream.GameId).GetAwaiter().GetResult();
            return new EmbedBuilder()
                .WithAuthor($"{streamer.Name} is live", url: subscription.AlternateLink ?? $"https://twitch.tv/{streamer.Name}")
                .WithTitle($"{stream.Title}")
                .WithUrl(subscription.AlternateLink ?? $"https://twitch.tv/{streamer.Name}")
                .WithThumbnailUrl(logoUrl)
                .AddField("Playing", string.IsNullOrWhiteSpace(game.Name) ? "No Game" : game.Name, true)
                .AddField("Viewers", stream.ViewerCount, true)
                //we add the timeseconds so the image wont be used from the cache 
                .WithImageUrl(
                    $"{stream.ThumbnailUrl.Replace("{width}", "1920").Replace("{height}", "1080")}?{DateTimeOffset.Now.ToUnixTimeSeconds()}")
                .WithFooter($"Live for {timeLive.Humanize(2, maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Second)}")
                .Build();
        }
    }
}