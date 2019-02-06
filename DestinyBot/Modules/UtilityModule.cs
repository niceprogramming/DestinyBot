using System;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using Discord.Commands;

namespace DestinyBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public Task Hello()
        {
            return ReplyAsync($"Hello {Context.User.Mention}");
        }

        [Command("kill")]
        [RequireOwnerOrAdmin]
        public async Task Kill()
        {
            await ReplyAsync("Please, I want to live");
            Environment.Exit(0);
        }
        [Command("restart")]
        [RequireOwnerOrAdmin]
        public async Task Restart()
        {
            await ReplyAsync("I'll be back");
            Environment.Exit(1);
        }
    }
}