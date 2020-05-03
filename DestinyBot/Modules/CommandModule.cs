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
        public async Task AddCommand(string name, [Remainder] string content)
        {
            await _customCommandService.AddCommand(name, content);
            await _customCommandService.BuiltCommandsAsync();
            await ReplyAsync($"{name} command created");
        }

        [Command("update")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task UpdateCommand(string name, [Remainder] string content)
        {
            await _customCommandService.UpdateCommand(name, content);
            await _customCommandService.BuiltCommandsAsync();
            await ReplyAsync($"{name} command updated");
        }


        [Command("remove")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task RemoveCommand(string name)
        {
            await _customCommandService.RemoveCommand(name);
            await _customCommandService.BuiltCommandsAsync();
            await ReplyAsync($"{name} command removed");
        }
    }
}