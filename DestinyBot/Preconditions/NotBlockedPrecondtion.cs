using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Services;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyBot.Preconditions
{
    class NotBlockedPrecondtion : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command,
            IServiceProvider services)
        {
            var isBlocked = services.GetService<BlockService>().IsBlocked(context.User.Id);

            return isBlocked ? PreconditionResult.FromError(String.Empty) : PreconditionResult.FromSuccess();
        }
    }


}
