using System.Net.Http;
using System.Threading.Tasks;
using DestinyBot.Models;
using Newtonsoft.Json;

namespace DestinyBot.Services
{
    public class FerretService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<FerretPicture> GetFerretPicture()
        {
            var response = await _httpClient.GetStringAsync("https://polecat.me/api/ferret");

            return JsonConvert.DeserializeObject<FerretPicture>(response);
        }
    }
}