using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Models;
using Discord;
using Discord.Commands;
using Serilog;

namespace DestinyBot.Preconditions
{
    public class ThrottleCommandAttribute : PreconditionAttribute
    {
        private readonly Dictionary<(ulong, string), DateTimeOffset> _throttles =
            new Dictionary<(ulong, string), DateTimeOffset>();


        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var expiryPeriod = TimeSpan.FromSeconds(45);
            if ((await context.Client.GetApplicationInfoAsync()).Owner.Username == context.User.Username)
            {
                return PreconditionResult.FromSuccess();
            }

            var user = context.User as IGuildUser;

            if (user.GuildPermissions.Administrator)
            {
                return PreconditionResult.FromSuccess();
            }

            var key = (context.Channel.Id, command.Name);
            var keyExist = _throttles.TryGetValue(key, out var time);
            if (!keyExist || DateTimeOffset.UtcNow > time)
            {
                Log.Information("{user} started throttle in {channel} for {command}", context.User.Username,
                    context.Channel.Name, command.Name);
                _throttles[key] = DateTimeOffset.UtcNow + expiryPeriod;
                return PreconditionResult.FromSuccess();
            }


            Log.Information("{user} throttled in {channel} for {command}", context.User.Username, context.Channel.Name,
                command.Name);
            return PreconditionResult.FromError(string.Empty);
        }
    }
}
