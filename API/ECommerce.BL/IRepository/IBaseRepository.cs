using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECommerce.BLL.Request;
using ECommerce.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ECommerce.BLL.IRepository
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task<T> AddAsync(T entity, Guid userId);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> Entitys);
        T Update(T Entity, Guid userId);
        T Delete(T entity, Guid userId);
        Task<T> FindAsync(int ID);
        Task<T> FindAsync(Guid ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(BaseGridRequest request, List<string> Includes = null);

        Task<IEnumerable<T>> GetAllAsync(string[] Includes = null);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null
        );
        Task<IEnumerable<T>> GetAllAsync(
            string[] Includes = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = Constants.OrderBY.Ascending
        );
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = Constants.OrderBY.Ascending
        );
        Task<T> GetItemAsync(Expression<Func<T, bool>> Criteria, string[] Includes = null);
        Task<string> UploadPhotoAsync(IFormFile file, string folderName, string photoName = null);
        Task<string> UploadPhotoAsync(Stream file, string folderName);
        Task<List<string>> UploadPhotos(
            List<IFormFile> Files,
            string FolderName,
            List<string> ImgNames = null
        );

        Task DeleteFile(string fullPath);

        bool ToggleActive(bool IsActive);
        List<string> SearchEntity();
        Task<T> FirstAsync();
        Task<T> FirstAsync(Expression<Func<T, bool>> Criteria);
        Task<T> FirstAsync(Expression<Func<T, bool>> Criteria, string[] Includes = null);
    }
}
