using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Jobs;
using DestinyBot.Models;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyBot.Modules
{
    [Group("twitter")]
    [Name("Twitter")]
    [RequireContext(ContextType.Guild)]
    public class TwitterModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _botContext;
        private readonly BotConfig _config;
        private readonly IServiceProvider _provider;
        private readonly TwitterService _twitterService;

        public TwitterModule(TwitterService twitterService, DestinyBotContext botContext, BotConfig config,
            IServiceProvider provider)
        {
            _twitterService = twitterService;
            _botContext = botContext;
            _config = config;
            _provider = provider;
        }

        [Command]
        [NotBlockedPrecondtion]
        [ThrottleCommand]
        [Remarks("?twitter")]
        public async Task Twitter()
        {
            var owner = await _botContext.GuildOwners.FirstOrDefaultAsync(x =>
                Context.Guild.Id.ToString() == x.GuildId);

            if (owner is null || owner.TwitchId == 0) return;


            var tweet = await _twitterService.GetLatestTweetForUserAsync(owner.TwitterUserId);

            await ReplyAsync($@"https://twitter.com/{tweet.User.ScreenName}/status/{tweet.Id}");
        }

        [Command("add")]
        [RequireOwnerOrAdmin]
        [Summary("Add twitter account to follow in a specified channel")]
        [Remarks("?twitter add destiny #destinyhub")]
        public async Task AddTwitter(string twitterName, IGuildChannel guildChannel)
        {
            var userResponse = await _twitterService.GetUserFromHandle(twitterName);


            if (userResponse?.Id is null)
            {
                await ReplyAsync("User Not Found");
                return;
            }

            var user = await _botContext.TwitterUsers.Include(x => x.TwitterSubscriptions)
                .FirstOrDefaultAsync(x => x.Id == userResponse.Id);
            if (user is null)
            {
                user = new TwitterUser
                {
                    Id = userResponse.Id.Value,
                    Name = userResponse.Name,
                    TwitterSubscriptions = new List<TwitterSubscription>(),
                    ScreenName = userResponse.ScreenName
                };
                _botContext.TwitterUsers.Add(user);
                JobManager.AddJob(
                    () => new TwitterJob(user.Id, _twitterService,
                        _provider.GetService<DestinyBotContext>(), Context.Client).Execute(),
                    s => s.WithName(userResponse.ScreenName).ToRunEvery(30).Seconds());
            }

            if (user.TwitterSubscriptions.Any(x => x.DiscordChannelId == (long)guildChannel.Id))
            {
                await ReplyAsync($"You already subscribed to {user.ScreenName} in {guildChannel.Name}");
                return;
            }

            user.TwitterSubscriptions.Add(new TwitterSubscription
            {
                DiscordChannelId = (long)guildChannel.Id,
                TwitterUserId = user.Id
            });
            _botContext.GuildOwners.FirstOrDefault(x => x.GuildId.ToString() == x.GuildId).TwitterUserId = user.Id;
            var changes = _botContext.SaveChanges();

            if (changes > 0)
                await ReplyAsync($"Alert for {user.ScreenName} added to {guildChannel.Name}");
            else
                await ReplyAsync($"Unable to create Alert for {user.ScreenName}");
        }

        [Command("remove")]
        [RequireOwnerOrAdmin]
        [Summary("Remove twitter account follow in a specified channel")]
        [Remarks("?twitter remove destiny #destinyhub")]
        public async Task RemoveTwitter(string twitterName, IGuildChannel guildChannel)
        {
            var twitter = await _botContext.TwitterUsers.Include(x => x.TwitterSubscriptions).FirstOrDefaultAsync(
                x =>
                    x.ScreenName.Equals(twitterName, StringComparison.CurrentCultureIgnoreCase));

            if (twitter is null)
            {
                await ReplyAsync("Twitter Not Found");
                return;
            }

            var alert = twitter.TwitterSubscriptions.FirstOrDefault(x =>
                x.DiscordChannelId == (long)guildChannel.Id);
            if (alert is null)
            {
                await ReplyAsync($"This channel doesnt contain an alert for {twitter.ScreenName}");
                return;
            }

            twitter.TwitterSubscriptions.Remove(alert);
            var guildOwner =
                await _botContext.GuildOwners.FirstOrDefaultAsync(x => Context.Guild.Id.ToString() == x.GuildId);
            if (guildOwner.TwitterUserId == twitter.Id)
            {
                var owner = await _botContext.GuildOwners.FindAsync(guildOwner.Snowflake,
                    guildOwner.GuildId);
                owner.TwitterUserId = 0;
            }

            var changes = _botContext.SaveChanges();

            if (changes > 0)
                await ReplyAsync($"Alert for {twitter.ScreenName} removed from {guildChannel.Name}");
            else
                await ReplyAsync($"Unable to remove Alert for {twitter.ScreenName}");
        }

        protected override void AfterExecute(CommandInfo command)
        {
            _botContext.Dispose();
        }
    }
}