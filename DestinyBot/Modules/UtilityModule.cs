using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
