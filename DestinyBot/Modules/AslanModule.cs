using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class AslanModule : ModuleBase<SocketCommandContext>
    {
        private readonly string _aslanImgur = "3MZVA";

        private readonly HttpClient _client;
        private readonly RandomImageService _randomImageService;

        public AslanModule(RandomImageService randomImageService, HttpClient client)
        {
            _randomImageService = randomImageService;

            _client = client;
        }

        [Command("aslan", RunMode = RunMode.Async)]
        [Alias("cat")]
        [NotBlockedPrecondtion]
        [ThrottleCommand]
        public async Task Aslan([Remainder] string question = null)
        {
            var image = await _randomImageService.GetRandomImage(_aslanImgur);

            var stream = await _client.GetStreamAsync(image.Link);

            await Context.Channel.SendFileAsync(stream, image.Link.Split('/').Last());
        }
    }
}