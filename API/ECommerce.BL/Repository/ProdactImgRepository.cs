using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Repository
{
    public class ProductPhotoRepository : BaseRepository<ProductPhoto>, IProductPhotoRepository
    {
        private readonly IHostingEnvironment hosting;

        public ProductPhotoRepository(Applicationdbcontext context, IHostingEnvironment hosting)
            : base(context, hosting) => this.hosting = hosting;

        public async Task<int> AddPhotos(Guid ID, IEnumerable<IFormFile> Files)
        {
            List<ProductPhoto> Photos = new();
            foreach (IFormFile file in Files)
            {
                Photos.Add(
                    new ProductPhoto
                    {
                        ProductID = ID,
                        PhotoPath = await UplodPhoto(file, Constants.PhotoFolder.ProductPhoto, null)
                    }
                );
            }
            _ = await AddaRangeAync(Photos);
            return Photos.Count;
        }

        public async Task<bool> DeletePhoto(string PhotoPath)
        {
            var Result = await GetItemAsync(i => i.PhotoPath == PhotoPath);
            if (Result == null)
                return false;
            else
            {
                string Filepath = Path.Combine(
                    hosting.ContentRootPath,
                    "Images",
                    Constants.PhotoFolder.ProductPhoto,
                    PhotoPath
                );
                File.Delete(Filepath);
                Delete(Result);
                return true;
            }
        }
    }
}
