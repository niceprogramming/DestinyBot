using System.Threading.Tasks;
using DestinyBot.Services;
using Discord;
using Discord.Commands;

namespace DestinyBot.Modules
{
    [Group("command")]
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly CustomCommandService _customCommandService;

        public CommandModule(CustomCommandService customCommandService)
        {
            _customCommandService = customCommandService;
        }

        [Command("add")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task AddCommand(string name, string content)
        {
            await _customCommandService.AddCommand(name, content);
            await _customCommandService.BuiltCommandsAsync();
        }
        
        [Command("remove")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task RemoveCommand(string name, string content)
        {
            await _customCommandService.AddCommand(name, content);
            await _customCommandService.BuiltCommandsAsync();
        }
    }
}