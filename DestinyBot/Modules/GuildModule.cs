using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class GuildModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _db;
        private readonly YoutubeService _youtube;

        public GuildModule(DestinyBotContext db, YoutubeService youtube)
        {
            _db = db;
            _youtube = youtube;
        }

        [RequireOwnerOrAdmin]
        [Command("setup", RunMode = RunMode.Async)]
        [Summary("Setup guild with default social media accounts.")]
        public async Task Setup(ITextChannel channel, string name)
        {
            var video = await _youtube.GetLatestVideoAsync(name);
            var owner = new GuildOwner
            {
                ChannelHub = channel.Id.ToString(),
                GuildId = channel.GuildId.ToString(),
                Snowflake = Context.Guild.OwnerId.ToString(),
                YoutubeId = video.Snippet.ChannelId
            };

            var youtubeSub = new YoutubeSubscription
            {
                ChannelId = video.Snippet.ChannelId,
                ChannelName = video.Snippet.ChannelTitle,
                DiscordChannelId = owner.ChannelHub,
                LatestVideoDate = video.Snippet.PublishedAt.ToUnixTimeSeconds()
            };

            _db.Add(owner);
            _db.Add(youtubeSub);

            await _db.SaveChangesAsync();
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            _db.Dispose();
        }
    }
}