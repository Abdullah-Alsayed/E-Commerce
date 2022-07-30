using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class ProdactImgRepository : BaseRepository<ProdactImg>, IProdactImgRepository
    {
        private readonly IHostingEnvironment hosting;

        public ProdactImgRepository(Applicationdbcontext context , IHostingEnvironment hosting) : base(context , hosting)
        {
            this.hosting = hosting;
        }
        public async Task<int> AddImgs(int ID , IEnumerable<IFormFile> Files)
        {
            List<ProdactImg> Imgs = new List<ProdactImg>();
            foreach (var file in Files)
            {
                Imgs.Add(new ProdactImg
                {
                    CrateAt = DateTime.Now,
                    ProdactID = ID,
                    Img = await UplodImge(file, Constants.ImgFolder.ProdactImg, null)
                });
            }
            await AddaRangeAync(Imgs);
            return Imgs.Count;
        }

        public async Task<bool> DeletImg(string Img)
        {
            var Result = await GetItemAsync(i => i.Img == Img);
            if (Result == null)
            {
                return false;
            }
            else
            {
                var Filepath = Path.Combine(hosting.ContentRootPath, "Images", Constants.ImgFolder.ProdactImg, Img);
                System.IO.File.Delete(Filepath);
                Delete(Result);
                return true;
            }
        }
    }
}
