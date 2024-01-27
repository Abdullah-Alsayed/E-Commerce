using ECommerce.BLL.Request;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task<T> AddaAync(T Entity);
        Task<IEnumerable<T>> AddaRangeAync(IEnumerable<T> Entitys);
        T Update(T Entity);
        bool Delete(T Entity);
        Task<T> FindAsync(int ID);
        Task<T> FindAsync(Guid ID);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(BaseGridRequest request);

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
        Task<string> UplodPhoto(IFormFile File, string FolderName, string ImgName = null);
        bool ToggleAvtive(bool IsActive);
        List<string> SearchEntity();
    }
}
