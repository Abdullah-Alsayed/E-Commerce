using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.Repository
{
    public class StatusRepository : BaseRepository<Status>, IStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusRepository(ApplicationDbContext context)
            : base(context) => _context = context;

        public async Task<int> UpdateAsync(Status status, int order, Guid userId)
        {
            var modifyRows = 0;
            var oldStatus = await FirstAsync(stat => !stat.IsDeleted && stat.Order == order, null);

            if (oldStatus != null && status.Order != order)
            {
                oldStatus.Order = status.Order;
                status.Order = order;
                modifyRows++;
            }
            status.ModifyAt = DateTime.UtcNow;
            status.ModifyBy = userId;
            modifyRows++;
            return modifyRows;
        }

        public async Task<int> AddAsync(Status status, Guid userId)
        {
            var modifyRows = 0;
            var order = (await GetAllAsync(stat => !stat.IsDeleted, null)).Count() + 1;

            status.Order = order;
            status.CreateAt = DateTime.UtcNow;
            status.CreateBy = userId;
            await _context.Statuses.AddAsync(status);
            modifyRows++;

            return modifyRows;
        }

        public async Task<int> MoveAsync(Status status, Guid statusId, Guid userId)
        {
            var modifyRows = 0;
            var oldStatus = await FirstAsync(stat => stat.Id == statusId);
            if (oldStatus != null && status.Order != oldStatus.Order)
            {
                var oldOrder = oldStatus.Order;
                oldStatus.Order = status.Order;
                status.Order = oldOrder;
                modifyRows++;
            }

            status.ModifyAt = DateTime.UtcNow;
            status.ModifyBy = userId;
            modifyRows++;

            return modifyRows;
        }

        public async Task<int> DeleteAsync(Status status, Guid userId)
        {
            var modifyRows = 0;
            var allStatus = await GetAllAsync(
                stat => !stat.IsDeleted && stat.Order > status.Order,
                null
            );

            foreach (var item in allStatus.ToList())
            {
                item.Order -= 1;
                modifyRows++;
            }
            status.DeletedAt = DateTime.UtcNow;
            status.DeletedBy = userId;
            status.IsDeleted = true;
            modifyRows++;
            return modifyRows;
        }
    }
}
