using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Services;
using Discord;
using Discord.WebSocket;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DestinyBot.Jobs
{
    public class TwitterJob : IJob
    {
        private readonly DestinyBotContext _botContext;
        private readonly DiscordSocketClient _client;
        private readonly TwitterService _twitterService;
        private readonly long _twitterUserId;

        public TwitterJob(long twitterUserId, TwitterService twitterService, DestinyBotContext botContext,
            DiscordSocketClient client)
        {
            _twitterUserId = twitterUserId;
            _twitterService = twitterService;
            _botContext = botContext;
            _client = client;
        }

        public void Execute()
        {
            using (_botContext)
            {
                var user = _botContext.TwitterUsers.Include(x => x.TwitterSubscriptions)
                    .FirstOrDefault(x => x.Id == _twitterUserId);
                if (user is null)
                {
                    return;
                }

                if (user.LastTweetId == 0)
                {
                    var tweet = _twitterService.GetLatestTweetForUserAsync(_twitterUserId).GetAwaiter().GetResult();
                    foreach (var twitterAlert in user.TwitterSubscriptions)
                    {
                        var channel = _client.GetChannel((ulong)twitterAlert.DiscordChannelId) as ITextChannel;
                        channel.SendMessageAsync($@"https://twitter.com/{user.ScreenName}/status/{tweet.Id}")
                            .GetAwaiter().GetResult();
                    }

                    Log.Information("{date}: {tweet}", tweet.CreatedAt, tweet.FullText ?? tweet.Text);
                    user.LastTweetId = tweet.Id;
                }
                else
                {
                    var tweets = _twitterService.GetTweetsSinceAsync(user.Id, user.LastTweetId).GetAwaiter().GetResult();

                    if (!tweets.Any())
                    {
                        return;
                    }

                    foreach (var twitterAlert in user.TwitterSubscriptions)
                    {
                        var channel = _client.GetChannel((ulong)twitterAlert.DiscordChannelId) as ITextChannel;

                        foreach (var tweet in tweets)
                        {
                            channel.SendMessageAsync($@"https://twitter.com/{user.ScreenName}/status/{tweet.Id}")
                                .GetAwaiter().GetResult();
                            Log.Information("{date}: {tweet}", tweet.CreatedAt, tweet.FullText);
                        }
                    }


                    user.LastTweetId = tweets.Last().Id;
                }

                _botContext.SaveChanges();
            }
        }
    }
}
