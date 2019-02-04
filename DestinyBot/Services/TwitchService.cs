using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Models.Twitch;
using Newtonsoft.Json;

namespace DestinyBot.Services
{
    public class TwitchService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TwitchService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.twitch.tv/helix/")
            };
            _httpClient.DefaultRequestHeaders.Add("Client-ID", _apiKey);
        }

        public async Task<bool> IsStreamLiveAsync(string username)
        {
            var response = await GetStreamAsync(username);
            return response != null;
        }

        public async Task<User> GetUserAsync(string username)
        {
            var response = await _httpClient.GetStringAsync($"users?login={username}");
            var json = JsonConvert.DeserializeObject<TwitchUserResponse>(response);

            return json.Users.FirstOrDefault();
        }

        public async Task<Stream> GetStreamAsync(string username)
        {
            var response = await _httpClient.GetStringAsync($"streams?user_login={username}");
            var json = JsonConvert.DeserializeObject<TwitchStreamResponse>(response);

            return json.Streams.FirstOrDefault();
        }

        public async Task<TwitchGame> GetGame(int id)
        {
            var response = await _httpClient.GetStringAsync($"games?id={id}");
            var data = JsonConvert.DeserializeObject<TwitchGameResponse>(response);

            return data.Games.FirstOrDefault();
        }
    }
}