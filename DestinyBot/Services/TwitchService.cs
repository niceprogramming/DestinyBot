using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Models.Twitch;
using Newtonsoft.Json;
using TwitchLib.Api;

namespace DestinyBot.Services
{
    public class TwitchService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private TwitchAPI _api;

        public TwitchService(string apiKey, string configTwitchClientSecret)
        {
            _api = new TwitchAPI();
            _api.Settings.ClientId = apiKey;
            _api.Settings.AccessToken = configTwitchClientSecret;
            _apiKey = apiKey;
            
        }

        public async Task<bool> IsStreamLiveAsync(string username)
        {
            
            var response = await GetStreamAsync(username);
            return response != null;
        }

        public async Task<User> GetUserAsync(string username)
        {
            var users =  await _api.Helix.Users.GetUsersAsync(logins: new List<string> {username});
            var user = users.Users.FirstOrDefault();
            if (user is null)
            {
                return null;
            }
            var twitchUser = new User()
            {
                Id = user.Id,
                BroadcasterType = user.BroadcasterType,
                Description = user.Description,
                DisplayName = user.DisplayName,
                Login = user.Login,
                OfflineImageUrl = user.OfflineImageUrl,
                ProfileImageUrl = user.ProfileImageUrl,
                Type = user.Type,
                ViewCount = user.ViewCount
            };

            return twitchUser;
        }

        public async Task<Stream> GetStreamAsync(string username)
        {
            var streams = await _api.Helix.Streams.GetStreamsAsync(userLogins: new List<string> {username});
            var stream = streams.Streams.FirstOrDefault();

            //don't care didn't ask plus you're white
            if (stream is null)
            {
                return null;
            }

            var result = new Stream()
            {
                Id = stream.Id,
                GameId = stream.GameId,
                CommunityIds = stream.CommunityIds.ToList(),
                Language = stream.Language,
                StartedAt = stream.StartedAt,
                Type = stream.Type,
                ThumbnailUrl = stream.ThumbnailUrl,
                Title = stream.Title,
                UserId = stream.UserId,
                UserName = stream.UserName,
                ViewerCount = stream.ViewerCount


            };

            return result;
        }

        public async Task<TwitchGame> GetGame(string id)
        {
            var response = await _api.Helix.Games.GetGamesAsync(new List<string> {id});

            var games = response.Games.FirstOrDefault();

            if (games is null)
            {
                return null;
            }

            var game = new TwitchGame()
            {
                Id = games.Id,
                BoxArtUrl = games.BoxArtUrl,
                Name = games.Name
            };

            return game;
        }
    }
}