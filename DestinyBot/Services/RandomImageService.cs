using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DestinyBot.Models;

namespace DestinyBot.Services
{
    public class RandomImageService
    {
        private readonly ImgurService _imgur;

        private readonly ConcurrentDictionary<string, List<Image>> _imageCache =
            new ConcurrentDictionary<string, List<Image>>();


        public RandomImageService(ImgurService imgur)
        {
            _imgur = imgur;
        }

        public async Task<Image> GetRandomImage(string albumId)
        {
            if (_imageCache.TryGetValue(albumId, out var cachedImages)) return cachedImages.Random();
            var images = (await _imgur.GetAlbumAsync(albumId)).ImgurData.Images;
            if (_imageCache.TryAdd(albumId, images)) ;
            {
                return images.Random();
            }
        }
    }
}