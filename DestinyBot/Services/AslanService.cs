using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyBot.Models;

namespace DestinyBot.Services
{
    public class AslanService
    {
        private readonly ImgurService _imgur;
        private List<Image> imageCache = new List<Image>();
        private const string album = "3MZVA";

        public AslanService(ImgurService imgur)
        {
            _imgur = imgur;
        }

        public async Task<Image> GetRandomImage()
        {
            if (imageCache.Count > 0)
            {
                return imageCache.Random();
            }

            imageCache = (await _imgur.GetAlbumAsync(album)).ImgurData.Images;
            return imageCache.Random();
        }


    }
}
