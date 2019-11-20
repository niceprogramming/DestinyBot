using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Services;
using Discord;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class BlockModule : ModuleBase<SocketCommandContext>
    {
        private readonly BlockService _blockService;

        public BlockModule(BlockService blockService)
        {
            _blockService = blockService;
        }
        [Command("block")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Block(IUser user)
        {
            var result = await _blockService.BlockUser(user.Id);
            if (result)
            {
                await ReplyAsync($"{user.Mention} was successfully blocked");
            }
            else
            {
                await ReplyAsync($"{user.Mention} was unable to be blocked");

            }

        }

        [Command("unblock")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Unblock(IUser user)
        {
            var result = await _blockService.UnblockUser(user.Id);
            if (result)
            {
                await ReplyAsync($"{user.Mention} was successfully unblocked");
            }
            else
            {
                await ReplyAsync($"{user.Mention} was unable to be unblocked");

            }

        }
    }
}
