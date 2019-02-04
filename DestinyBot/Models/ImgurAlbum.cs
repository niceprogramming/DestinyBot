using System.Collections.Generic;
using Newtonsoft.Json;

namespace DestinyBot.Models
{
    public class ImgurAlbum
    {
        [JsonProperty("data")] public Data ImgurData { get; set; }

        [JsonProperty("success")] public bool Success { get; set; }

        [JsonProperty("status")] public long Status { get; set; }
    }

    public class Data
    {
        [JsonProperty("images")] public List<Image> Images { get; set; }
    }
}