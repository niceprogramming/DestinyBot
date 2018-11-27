using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Models.Twitch;

namespace DestinyBot.Services
{
    public class TwitchService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TwitchService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.twitch.tv/helix/")
            };
        }

        
    }
}
