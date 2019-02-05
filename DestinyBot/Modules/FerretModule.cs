using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord.Commands;

namespace DestinyBot.Modules
{
    [Name("Ferret")]
    public class FerretModule : ModuleBase<SocketCommandContext>
    {
        private readonly FerretService _ferretService;
        private readonly HttpClient _httpClient;

        public FerretModule(FerretService ferretService, HttpClient httpClient)
        {
            _ferretService = ferretService;
            _httpClient = httpClient;
        }

        [Command("ferret", RunMode = RunMode.Async)]
        [ThrottleCommand]
        [Remarks("?ferret what should i do today")]
        public async Task Ferret([Remainder] string question = null)
        {
            var picture = await _ferretService.GetFerretPicture();

            var stream = await _httpClient.GetStreamAsync(picture.Url);

            await Context.Channel.SendFileAsync(stream, picture.Url.Split('/').Last());
        }
    }
}
