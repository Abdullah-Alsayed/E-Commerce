using System;
using System.Threading.Tasks;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IStatusRepository : IBaseRepository<Status>
    {
        public Task<int> UpdateAsync(Status status, int order, Guid userId);
        public Task<int> AddAsync(Status status, Guid userId);
        public Task<int> MoveAsync(Status status, Guid statusId, Guid userId);
        public Task<int> DeleteAsync(Status status, Guid userId);
    }
}
