using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Preconditions;
using Discord.Commands;
using Discord.Commands.Builders;
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

        public async Task UpdateCommand(string name, string content)
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                var command = context.CustomCommands.FirstOrDefault(x => x.Name == name);
                command.Content = content;
                await context.SaveChangesAsync();
            }
        }

        public async Task AddCommand(string name, string content)
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                context.CustomCommands.Add(new CustomCommand { Name = name, Content = content });
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveCommand(string name)
        {
            using (var context = _services.GetService<DestinyBotContext>())
            {
                context.CustomCommands.Remove(new CustomCommand { Name = name });
                await context.SaveChangesAsync();
            }
        }

        public async Task BuiltCommandsAsync()
        {
            var customCommands = new List<CustomCommand>();

            using (var context = _services.GetService<DestinyBotContext>())
                customCommands = context.CustomCommands.ToList();

            _customModule = await _commands.CreateModuleAsync(string.Empty, m =>
            {
                m.AddPrecondition(new NotBlockedPrecondtion());
                m.AddPrecondition(new ThrottleCommandAttribute());
                
                foreach (var command in customCommands)
                {
                    m.AddCommand(command.Name, async (ctx, _, _1, _2) =>
                    {
                        
                        await ctx.Channel.SendMessageAsync(command.Content);
                    }, CreateCommandBuilder());
                }
            });
        }

        // avoids getting hte customCommands and i in the clojure
        public Action<CommandBuilder> CreateCommandBuilder()
        {
            return new Action<CommandBuilder>(_ =>  _.AddParameter("extra", typeof(string), x => { x.AddAttributes(new RemainderAttribute()); x.WithDefault(null); x.IsOptional = true; }) );
        }
    }
}