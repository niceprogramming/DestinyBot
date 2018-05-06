using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using Humanizer;
using LittleSteve.Data;
using LittleSteve.Data.Entities;
using LittleSteve.Jobs;
using LittleSteve.Preconditions;
using LittleSteve.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeUnit = Humanizer.Localisation.TimeUnit;

namespace LittleSteve.Modules
{
    [Group("twitch")]
    [Alias("live")]

    [Name("Twitch")]
    [RequireContext(ContextType.Guild)]
    public class TwitchModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly SteveBotContext _botContext;
        private readonly IServiceProvider _provider;
        private readonly TwitchService _twitchService;


        public TwitchModule(TwitchService twitchService, SteveBotContext botContext, IServiceProvider provider)
        {
            _twitchService = twitchService;
            _botContext = botContext;
            _provider = provider;
        }

        [Command]
        [Blacklist]
        [ThrottleCommand]
        [Summary("View the status of the default Twitch streamer")]
        [Remarks("?twitch")]
       
        public async Task Twitch()
        {
            if (Context.GuildOwner is null || Context.GuildOwner.TwitchStreamerId == 0)
            {
                return;
            }


            var stream = await _twitchService.GetStreamAsync(Context.GuildOwner.TwitchStreamerId);
            var twitch =
                await _botContext.TwitchStreamers.FirstOrDefaultAsync(x => x.Id == Context.GuildOwner.TwitchStreamerId);
            if (stream is null)
            {
                var timeAgo = DateTimeOffset.UtcNow - twitch.StreamEndTime;
                await ReplyAsync(
                    $"**Stream Offline**\n{twitch.Name} was last seen {timeAgo.Humanize(3, minUnit: TimeUnit.Second, collectionSeparator: " ")} ago");
            }
            else
            {
                var timeLive = DateTimeOffset.UtcNow - twitch.SteamStartTime;
                var embed = new EmbedBuilder()
                    .WithAuthor($"{twitch.Name} is live", url: $"https://twitch.tv/{twitch.Name}")
                    .WithTitle($"{stream.Channel.Status}")
                    .WithUrl($"https://twitch.tv/{twitch.Name}")
                    .WithThumbnailUrl(stream.Channel.Logo)
                    .AddField("Playing", string.IsNullOrWhiteSpace(stream.Game) ? "No Game" : stream.Game, true)
                    .AddField("Viewers", stream.Viewers, true)
                    //we add the timeseconds so the image wont be used from the cache 
                    .WithImageUrl(
                        $"{stream.Preview.Template.Replace("{width}", "1920").Replace("{height}", "1080")}?{DateTimeOffset.Now.ToUnixTimeSeconds()}")
                    .WithFooter($"Live for {timeLive.Humanize(2, maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Second)}")
                    .Build();
                
                await ReplyAsync($"{twitch.AlternateStreamLink ?? string.Empty}", embed: embed);
            }
        }

        [Command("add")]
        [RequireOwnerOrAdmin]
        [Summary("Add twitch channel to follow in a specified channel")]
        [Remarks("?twitch add destiny #destinyhub")]
        public async Task AddTwitch(string twitchName, IGuildChannel guildChannel, string alternateLink )
        {
            var userResponse = await _twitchService.GetUserByNameAsync(twitchName);

            if (userResponse is null)
            {
                await ReplyAsync("User Not Found");
                return;
            }

            var user = await _botContext.TwitchStreamers.Include(x => x.TwitchAlertSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == long.Parse(userResponse.Id));

            if (user is null)
            {
                user = new TwitchStreamer
                {
                    Id = long.Parse(userResponse.Id),
                    Name = userResponse.DisplayName,
                    AlternateStreamLink = alternateLink,
                    TwitchAlertSubscriptions = new List<TwitchAlertSubscription>()
                };
                _botContext.TwitchStreamers.Add(user);
                JobManager.AddJob(
                    () => new TwitchMonitoringJob(user.Id, _twitchService,
                        _provider.GetService<SteveBotContext>(), Context.Client).Execute(),
                    s => s.WithName(userResponse.DisplayName).ToRunEvery(60).Seconds());
            }

            if (user.TwitchAlertSubscriptions.Any(x => x.DiscordChannelId == (long) guildChannel.Id))
            {
                await ReplyAsync($"You already subscribed to {user.Name} in {guildChannel.Name}");
                return;
            }

            user.TwitchAlertSubscriptions.Add(new TwitchAlertSubscription
            {
                DiscordChannelId = (long) guildChannel.Id,
                TwitchStreamerId = user.Id
            });

            var changes = _botContext.SaveChanges();

            if (changes > 0)
            {
                await ReplyAsync($"Alert for {user.Name} added to {guildChannel.Name}");
            }
            else
            {
                await ReplyAsync($"Unable to create Alert for {user.Name}");
            }
        }

        [Command("remove")]
        [RequireOwnerOrAdmin]
        [Summary("Removed followed Twitch channel from channel")]
        [Remarks("?twitch remove destiny #destinyhub")]
        public async Task RemoveTwitch(string twitchName, IGuildChannel guildChannel)
        {
            var user = await _botContext.TwitchStreamers.Include(x => x.TwitchAlertSubscriptions)
                .FirstOrDefaultAsync(x => x.Name.Equals(twitchName, StringComparison.CurrentCultureIgnoreCase));

            if (user is null)
            {
                await ReplyAsync("User Not Found");
                return;
            }

            var alert = user.TwitchAlertSubscriptions.FirstOrDefault(x => x.DiscordChannelId == (long) guildChannel.Id);
            if (alert is null)
            {
                await ReplyAsync($"This channel doesnt contain an alert for {user.Name}");
                return;
            }

            user.TwitchAlertSubscriptions.Remove(alert);
            if (Context.GuildOwner.TwitchStreamerId == user.Id)
            {
                var owner = await _botContext.GuildOwners.FindAsync(Context.GuildOwner.DiscordId,
                    Context.GuildOwner.GuildId);
                owner.TwitchStreamerId = 0;
            }


            var changes = _botContext.SaveChanges();

            if (changes > 0)
            {
                await ReplyAsync($"Alert for {user.Name} removed from {guildChannel.Name}");
            }
            else
            {
                await ReplyAsync($"Unable to remove Alert for {user.Name}");
            }
        }

        protected override void AfterExecute(CommandInfo command)
        {
            _botContext.Dispose();
        }
    }
}