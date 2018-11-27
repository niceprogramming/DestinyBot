using System.Threading.Tasks;
using Discord.Commands;

namespace DestinyBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("hello")]
        public Task Hello() => ReplyAsync($"Hello {Context.User.Mention}");
    }
}