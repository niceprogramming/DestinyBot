using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Models;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class AslanModule : ModuleBase<SocketCommandContext>
    {

        private readonly HttpClient _client;
        private readonly BotConfig _config;
        private readonly AslanService _aslanService;

        public AslanModule(AslanService aslanService, BotConfig config, HttpClient client)
        {
            _aslanService = aslanService;
            _config = config;
            _client = client;
        }

        [Command("aslan", RunMode = RunMode.Async)]
        [Alias("cat")]
        [ThrottleCommand]
        public async Task Aslan([Remainder] string question = null)
        {
            var image = await _aslanService.GetRandomImage();

            var stream = await _client.GetStreamAsync(image.Link);
            
            await Context.Channel.SendFileAsync(stream, image.Link.Split('/').Last());
        }
    }
}
