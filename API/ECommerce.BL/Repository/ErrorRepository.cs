using System;
using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Repository
{
    public class ErrorRepository : BaseRepository<ErrorLog>, IErrorRepository
    {
        private readonly Applicationdbcontext _context;

        public ErrorRepository(Applicationdbcontext context)
            : base(context)
        {
            _context = context;
        }

        public async Task ErrorLog(Exception ex, OperationTypeEnum action, EntitiesEnum entity)
        {
            _context.ChangeTracker.Clear();
            await AddaAync(
                new ErrorLog
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    Operation = action,
                    Entity = entity
                }
            );
        }
    }
}
