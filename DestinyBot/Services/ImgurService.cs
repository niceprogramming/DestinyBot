using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Models;
using Newtonsoft.Json;

namespace DestinyBot.Services
{
    public class ImgurService
    {
        private readonly string _clientId;
        private readonly HttpClient _httpClient = new HttpClient();

        public ImgurService(string clientId)
        {
            _clientId = clientId;

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {_clientId}");
        }

        public async Task<ImgurAlbum> GetAlbumAsync(string albumId)
        {
            var response = await _httpClient.GetStringAsync($"https://api.imgur.com/3/album/{albumId}");

            return JsonConvert.DeserializeObject<ImgurAlbum>(response);
        }
    }
}