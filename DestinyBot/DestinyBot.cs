using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DestinyBot.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                .Configure<BotConfig>(_config)
                .BuildServiceProvider();
        }


    }
}
