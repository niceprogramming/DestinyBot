using System.Collections.Generic;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using Serilog;

namespace DestinyBot.Modules
{
    public class GuildModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _db;
        private readonly TwitchService _twitchService;
        private readonly YoutubeService _youtube;

        public GuildModule(DestinyBotContext db, YoutubeService youtube, TwitchService twitchService)
        {
            _db = db;
            _youtube = youtube;
            _twitchService = twitchService;
        }

        [RequireOwner]
        [Command("setup", RunMode = RunMode.Async)]
        [Summary("Setup guild with default social media accounts.")]
        public async Task Setup(ITextChannel textChannel, string name)
        {
            var video = await _youtube.GetLatestVideoAsync(name);
            var user = await _twitchService.GetUserAsync(name);
            var owner = new GuildOwner
            {
                ChannelHub = textChannel.Id.ToString(),
                GuildId = textChannel.GuildId.ToString(),
                Snowflake = Context.Guild.OwnerId.ToString(),
                YoutubeId = video.Snippet.ChannelId
            };

            var youtubeSub = new YoutubeSubscription
            {
                DiscordChannelId = owner.ChannelHub
            };
            var channel = new Youtube
            {
                Id = video.Snippet.ChannelId,
                Name = video.Snippet.ChannelTitle,
                LatestVideoDate = video.Snippet.PublishedAt.ToUnixTimeSeconds(),
                YoutubeSubscriptions = new List<YoutubeSubscription> {youtubeSub}
            };
            Log.Information($"{channel.Name}");
            var twitchSub = new TwitchSubscription
            {
                DiscordChannelId = owner.ChannelHub
            };
            var streamer = new TwitchStreamer
            {
                Id = user.Id,
                Name = user.DisplayName,
                TwitchSubscriptions = new List<TwitchSubscription> {twitchSub}
            };
            Log.Information($"{streamer.Name}");
            _db.Add(owner);
            _db.Add(channel);
            _db.Add(streamer);

            await _db.SaveChangesAsync();
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            _db.Dispose();
        }
    }
}