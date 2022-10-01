using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IProdactImgRepository:IBaseRepository<ProdactImg>
    {
        Task<int> AddImgs(int ID, IEnumerable<IFormFile> Files);
        Task<bool> DeletImg(string Img);
    }
}
