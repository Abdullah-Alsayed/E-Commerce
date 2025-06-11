using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ContentModule
{
    public class ContentModule : IContentModule
    {
        private readonly ApplicationDbContext _context;

        public ContentModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBaseRepository<Feedback> Feedback =>
            _feedback ??= new BaseRepository<Feedback>(_context);
        private IBaseRepository<Feedback> _feedback;

        public IBaseRepository<ContactUs> ContactUs =>
            _contactUs ??= new BaseRepository<ContactUs>(_context);
        private IBaseRepository<ContactUs> _contactUs;

        public IBaseRepository<History> History =>
            _history ??= new BaseRepository<History>(_context);
        private IBaseRepository<History> _history;

        public INotificationRepository Notification =>
            _notification ??= new NotificationRepository(_context);
        private INotificationRepository _notification;

        public IErrorRepository ErrorLog => _errorLog ??= new ErrorRepository(_context);
        private IErrorRepository _errorLog;
    }
}
