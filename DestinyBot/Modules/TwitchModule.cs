using System;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace DestinyBot.Modules
{
    [Alias("live")]
    [Name("Twitch")]
    [RequireContext(ContextType.Guild)]
    public class TwitchModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _db;

        private readonly TwitchService _twitchService;


        public TwitchModule(TwitchService twitchService, DestinyBotContext botContext)
        {
            _twitchService = twitchService;
            _db = botContext;
        }

        [Command]
        [ThrottleCommand]
        public async Task Twitch()
        {
            var owner = await _db.GuildOwners.FirstOrDefaultAsync(x => Context.Guild.Id.ToString() == x.GuildId);

            if (owner is null || owner.TwitchId == 0) return;
            var twitch =
                await _db.TwitchStreamers.FirstOrDefaultAsync(x => x.Id == owner.TwitchId);
            var streamTask = _twitchService.GetStreamAsync(twitch.Name);
            var logo = _twitchService.GetUserAsync(twitch.Name);

            if (await streamTask is null)
            {
                var timeAgo = DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(twitch.StreamEndTime);
                await ReplyAsync(
                    $"**Stream Offline**\n{twitch.Name} was last seen {timeAgo.Humanize(3, minUnit: TimeUnit.Second, collectionSeparator: " ")} ago");
            }
            else
            {
                var stream = await streamTask;
                var game = await _twitchService.GetGame(stream.GameId);
                var timeLive = DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(twitch.SteamStartTime);
                var embed = new EmbedBuilder()
                    .WithAuthor($"{twitch.Name} is live", url: $"https://twitch.tv/{twitch.Name}")
                    .WithTitle($"{stream.Title}")
                    .WithUrl($"https://twitch.tv/{twitch.Name}")
                    .WithThumbnailUrl((await logo).ProfileImageUrl)
                    .AddField("Playing", string.IsNullOrWhiteSpace(game?.Name) ? "No Game" : game.Name, true)
                    .AddField("Viewers", stream.ViewerCount, true)
                    //we add the timeseconds so the image wont be used from the cache 
                    .WithImageUrl(
                        $"{stream.ThumbnailUrl.Replace("{width}", "1920").Replace("{height}", "1080")}?{DateTimeOffset.Now.ToUnixTimeSeconds()}")
                    .WithFooter($"Live for {timeLive.Humanize(2, maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Second)}")
                    .Build();

                await ReplyAsync(" ", embed: embed);
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            base.AfterExecute(command);
            _db.Dispose();
        }
    }
}