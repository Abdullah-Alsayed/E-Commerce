using System;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ContentModule
{
    public class ContentModule : IContentModule
    {
        private readonly ApplicationDbContext _context;

        private readonly Lazy<INotificationRepository> _notification;
        private readonly Lazy<IBaseRepository<History>> _history;
        private readonly Lazy<IErrorRepository> _errorLog;
        private readonly Lazy<IBaseRepository<Feedback>> _feedback;
        private readonly Lazy<IBaseRepository<ContactUs>> _contactUs;

        public ContentModule(ApplicationDbContext context)
        {
            _context = context;
            _feedback = new Lazy<IBaseRepository<Feedback>>(
                () => new BaseRepository<Feedback>(_context)
            );
            _contactUs = new Lazy<IBaseRepository<ContactUs>>(
                () => new BaseRepository<ContactUs>(_context)
            );

            _notification = new Lazy<INotificationRepository>(
                () => new NotificationRepository(_context)
            );
            _history = new Lazy<IBaseRepository<History>>(
                () => new BaseRepository<History>(_context)
            );
            _errorLog = new Lazy<IErrorRepository>(() => new ErrorRepository(_context));
        }

        public IBaseRepository<Feedback> Feedback => _feedback.Value;
        public IBaseRepository<ContactUs> ContactUs => _contactUs.Value;
        public INotificationRepository Notification => _notification.Value;
        public IBaseRepository<History> History => _history.Value;
        public IErrorRepository ErrorLog => _errorLog.Value;
    }
}
