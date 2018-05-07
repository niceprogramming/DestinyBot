using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Extensions;
using LittleSteve.Models;
using LittleSteve.Preconditions;
using LittleSteve.Services;

namespace LittleSteve.Modules
{
    [Name("RMS")]
    public class RMSModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly HttpClient _client;
        private readonly BotConfig _config;
        private readonly ImgurService _imgurService;

        public RMSModule(ImgurService imgurService, BotConfig config, HttpClient client)
        {
            _imgurService = imgurService;
            _config = config;
            _client = client;
        }

        [Command("rms", RunMode = RunMode.Async)]
        [Alias("stallman")]
        [Blacklist]
        [BlockChannels()]
        [ThrottleCommand]
        [Summary("Get a picture of Richard Stallman")]
        [Remarks("?rms What is freedom")]
        public async Task RMS([Remainder] string question = null)
        {
            var album = await _imgurService.GetAlbumAsync(_config.RMSAlbumId);

            var image = album.ImgurData.Images.Random();


            var stream = await _client.GetStreamAsync(image.Link);
            await Context.Channel.SendFileAsync(stream, image.Link.Split('/').Last());
        }
    }
}