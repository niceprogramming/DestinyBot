using System;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Jobs;
using DestinyBot.Models;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace DestinyBot
{
    public class DestinyBot
    {
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;

        public DestinyBot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 8000,
                AlwaysDownloadUsers = true,
#if DEBUG
                LogLevel = LogSeverity.Verbose,
#else
                LogLevel = LogSeverity.Info,
#endif
                DefaultRetryMode = RetryMode.AlwaysRetry
            });

            _config = BuildConfig();
            _services = ConfigureServices();
        }

        public async Task StartAsync()
        {
            _client.Log += BotLogHook.Log;

            await _client.LoginAsync(TokenType.Bot, _config.Get<BotConfig>().DiscordToken);

            await _client.StartAsync();

            await Task.Delay(5000);

            SetupJobs();

            await _services.GetRequiredService<CommandHandlingService>().StartAsync(_services);
            await _services.GetRequiredService<ReminderService>().StartAsync(_services);
            await _services.GetRequiredService<CustomCommandService>().StartAsync(_services);

            await Task.Delay(-1);
        }


        private void SetupJobs()
        {
            var registry = new Registry();
            registry.NonReentrantAsDefault();
            using (var db = _services.GetRequiredService<DestinyBotContext>())
            {
                db.Database.Migrate();

                foreach (var subscription in db.Channels)
                    registry.Schedule(() => new YoutubeJob(
                            subscription.Name,
                            _services.GetService<YoutubeService>(),
                            _client,
                            _services.GetService<DestinyBotContext>()))
                        .WithName(subscription.Id)
                        .ToRunNow().AndEvery(5)
                        .Minutes();

                foreach (var streamer in db.TwitchStreamers)
                    registry.Schedule(() => new TwitchJob(
                            streamer.Id,
                            _services.GetService<TwitchService>(),
                            _services.GetService<DestinyBotContext>(),
                            _client))
                        .WithName(streamer.Id.ToString())
                        .ToRunNow().AndEvery(1)
                        .Minutes();

                foreach (var twitterUser in db.TwitterUsers)
                    registry.Schedule(() => new TwitterJob(
                            twitterUser.Id,
                            _services.GetService<TwitterService>(),
                            _services.GetService<DestinyBotContext>(),
                            _client))
                        .WithName(twitterUser.Id.ToString())
                        .ToRunNow().AndEvery(3)
                        .Minutes();
            }

            JobManager.Initialize(registry);
            JobManager.JobException += info => Log.Information(info.Exception, "{jobName} has a problem", info.Name);
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables(string.Empty)
                .Build();
        }

        private IServiceProvider ConfigureServices()
        {
            var config = _config.Get<BotConfig>();
            var stuff = Environment.GetEnvironmentVariables();
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<CustomCommandService>()
                .AddSingleton<RandomImageService>()
                .AddScoped(s => new ImgurService(config.ImgurClientId))
                .AddSingleton(new TwitterService(config.TwitterConsumerKey, config.TwitterConsumerSecret,
                    config.TwitterAccessToken, config.TwitterAccessSecret))
                .AddSingleton<FerretService>()
                .Configure<BotConfig>(_config)
                .AddSingleton(new YoutubeService(config.YoutubeKey))
                .AddSingleton(new TwitchService(config.TwitchClientId)).AddSingleton<ReminderService>()
#if DEBUG
                .AddLogging(b => b.AddSerilog(dispose: true))
#endif
                .AddDbContext<DestinyBotContext>(ServiceLifetime.Transient)
                //We delegate the config object so we dont have to use IOptionsSnapshot<T> or IOptions<T> to get the Config
                .AddTransient(provider => provider.GetRequiredService<IOptions<BotConfig>>().Value)
                .BuildServiceProvider();
        }
    }
}