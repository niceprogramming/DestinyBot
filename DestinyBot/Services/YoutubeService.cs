using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Models.Youtube;
using Newtonsoft.Json;

namespace DestinyBot.Services
{
    public class YoutubeService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public YoutubeService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/")
            };
        }

        public async Task<string> GetUploadPlaylistAsync(string user)
        {
            var response =
                await _httpClient.GetStringAsync(
                    $"channels?part=snippet,contentDetails&forUsername={user}&key={_apiKey}");
            var json = JsonConvert.DeserializeObject<YoutubeChannelResponse>(response);

            return json.YoutubeChannels.FirstOrDefault()?.ContentDetails.RelatedPlaylists.Uploads;
        }

        public async Task<IEnumerable<YoutubeVideo>> GetVideosForUserAsync(string user)
        {
            var id = await GetUploadPlaylistAsync(user);
            if (id is null)
            {
                return new List<YoutubeVideo>();
            }

            var response =
                await _httpClient.GetStringAsync(
                    $"playlistItems?part=snippet,contentDetails&playlistId={id}&key={_apiKey}");

            var json = JsonConvert.DeserializeObject<YoutubePlaylistResponse>(response);
            return json.YoutubeVideos;
        }

        public async Task<YoutubeVideo> GetLatestVideoAsync(string user) =>
            (await GetVideosForUserAsync(user))?.FirstOrDefault();
    }
}