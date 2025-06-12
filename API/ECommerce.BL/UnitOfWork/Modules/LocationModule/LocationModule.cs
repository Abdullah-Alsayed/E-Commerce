using System;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.LocationModule
{
    public class LocationModule : ILocationModule
    {
        private readonly ApplicationDbContext _context;

        // Replace private fields with Lazy<T>
        private readonly Lazy<IBaseRepository<Governorate>> _governorate;
        private readonly Lazy<IBaseRepository<Area>> _area;

        public LocationModule(ApplicationDbContext context)
        {
            _context = context;

            // Initialize Lazy<T> instances
            _governorate = new Lazy<IBaseRepository<Governorate>>(
                () => new BaseRepository<Governorate>(_context)
            );
            _area = new Lazy<IBaseRepository<Area>>(() => new BaseRepository<Area>(_context));
        }

        // Update properties to use Lazy<T>.Value
        public IBaseRepository<Governorate> Governorate => _governorate.Value;
        public IBaseRepository<Area> Area => _area.Value;
    }
}
