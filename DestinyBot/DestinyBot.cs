using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DestinyBot.Models;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
                MessageCacheSize = 5000,
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

            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync(_services);
            await Task.Delay(-1);
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

            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .Configure<BotConfig>(_config)

                //We delegate the config object so we dont have to use IOptionsSnapshot or IOptions in our code
                .AddTransient(provider => provider.GetRequiredService<IOptions<BotConfig>>().Value)
                .BuildServiceProvider();
        }


    }
}
