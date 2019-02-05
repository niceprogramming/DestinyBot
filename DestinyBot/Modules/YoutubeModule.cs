using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace DestinyBot.Modules
{
    [Name("youtube")]
    [Alias("yt")]
    public class YoutubeModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _db;
        private readonly YoutubeService _youtubeService;


        public YoutubeModule(DestinyBotContext db, YoutubeService youtubeService)
        {
            _db = db;
            _youtubeService = youtubeService;
        }

        [Command]
        [ThrottleCommand]
        public async Task Youtube()
        {
            var owner = await _db.GuildOwners.FirstOrDefaultAsync(x => Context.Guild.Id.ToString() == x.GuildId);
            if (string.IsNullOrWhiteSpace(owner?.YoutubeId)) return;

            var name = await _db.YoutubeSubscriptions.Include(x => x.Youtube)
                .FirstOrDefaultAsync(x => x.YoutubeId == owner.YoutubeId);
            
            var video = await _youtubeService.GetLatestVideoAsync(name.Youtube.Name);
            await ReplyAsync($"https://www.youtube.com/watch?v={video.Snippet.ResourceId.VideoId}");
        }
    }
}