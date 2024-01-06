using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IProductPhotoRepository : IBaseRepository<ProductPhoto>
    {
        Task<int> AddPhotos(Guid ID, IEnumerable<IFormFile> Files);
        Task<bool> DeletePhoto(string Photo);
    }
}
