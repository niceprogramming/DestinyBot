using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using DestinyBot.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace DestinyBot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly BotConfig _config;
        private IServiceProvider _provider;

        public CommandHandlingService(DiscordSocketClient client, CommandService commands, BotConfig config)
        {
            _client = client;
            _commands = commands;
            _config = config;
            _commands.Log += BotLogHook.Log;
            _commands.CommandExecuted += CommandExecuted;
            
            _client.MessageReceived += MessageReceived;
        }
        
        private async Task CommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3)
        {
            
            Log.Information("{User} executed {commandName} command" ,arg2.User.Username,arg1.Value.Name);
        }

        public async Task StartAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            
        }
        
        private async Task MessageReceived(SocketMessage rawMessage)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;

            if (message.Source != MessageSource.User) return;


            var argPos = 0;
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                  message.HasStringPrefix(_config.Prefix, ref argPos)))
                return;

            var context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, _provider);
            
            if (!result.IsSuccess) Log.Error($"{result.Error}: {result.ErrorReason}");

            stopwatch.Stop();
            Log.Information($"Took {stopwatch.ElapsedMilliseconds}ms to process: {message}");
        }
    }
}