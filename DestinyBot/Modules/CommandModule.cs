using System.Threading.Tasks;
using DestinyBot.Services;
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
        public async Task AddCommand(string name, string content)
        {
            await _customCommandService.AddCommand(name, content);
            await _customCommandService.BuiltCommandsAsync();
        }
    }
}