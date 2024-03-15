using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            try
            {
                _context.ChangeTracker.Clear();
                await AddaAync(
                    new ErrorLog
                    {
                        Source = ex.InnerException.Source,
                        Message = ex.InnerException.Message,
                        StackTrace = ex.InnerException.StackTrace,
                        Operation = action,
                        Entity = entity
                    }
                );
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.InnerException.Message);
            }
        }
    }
}
