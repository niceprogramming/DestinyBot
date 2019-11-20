using System;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using Discord.Commands;

namespace DestinyBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [NotBlockedPrecondtion]
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

        [Command("age")]

        public async Task Age()
        {
            var time = DateTimeOffset.FromUnixTimeMilliseconds((long)((Context.User.Id >> 22) + 1420070400000UL));
            await ReplyAsync(time.ToString("R"));

        }
    }
}