using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class StallmanModule : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _client;
        private readonly RandomImageService _randomImageService;
        private readonly string _rmsImgur = "7K5Gcmh";

        public StallmanModule(RandomImageService randomImageService, HttpClient client)
        {
            _randomImageService = randomImageService;

            _client = client;
        }

        [Command("rms", RunMode = RunMode.Async)]
        [Alias("stallman")]
        [NotBlockedPrecondtion]
        [ThrottleCommand]
        public async Task Stallman([Remainder] string question = null)
        {
            var image = await _randomImageService.GetRandomImage(_rmsImgur);

            var stream = await _client.GetStreamAsync(image.Link);

            await Context.Channel.SendFileAsync(stream, image.Link.Split('/').Last());
        }
    }
}