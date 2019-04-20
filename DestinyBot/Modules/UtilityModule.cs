using System;
using System.Threading.Tasks;
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
        [RequireOwner]
        public async Task Kill()
        {
            await ReplyAsync("Please, I want to live");
            Environment.Exit(0);
        }

        [Command("restart")]
        [RequireOwner]
        public async Task Restart()
        {
            await ReplyAsync("I'll be back");
            Environment.Exit(1);
        }
    }
}