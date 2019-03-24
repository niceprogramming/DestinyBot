using System;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyBot.Services
{
    public class CustomCommandService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private ModuleInfo _customModule;
        private IServiceProvider _services;

        public CustomCommandService(CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
            _client = client;
        }

        public async Task StartAsync(IServiceProvider services)
        {
            _services = services;
            await BuiltCommandsAsync();
        }

        public async Task AddCommand(string name, string content, params string[] aliases)
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                context.CustomCommands.Add(new CustomCommand {Name = name, Content = content, Aliases = aliases});
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveCommand(string name)
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                context.CustomCommands.Remove(new CustomCommand {Name = name});
                await context.SaveChangesAsync();
            }
        }

        public async Task BuiltCommandsAsync()
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                _customModule = await _commands.CreateModuleAsync(string.Empty, m =>
                {
                    foreach (var command in context.CustomCommands)
                        m.AddCommand(command.Name, async (ctx, _, provider, _1) =>
                        {
                            using (var db = provider.GetService<DestinyBotContext>())
                            {
                                await ctx.Channel.SendMessageAsync(command.Content);
                            }
                        }, c => { });
                });
            }
        }
    }
}