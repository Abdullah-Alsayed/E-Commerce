using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.LocationModule
{
    public class LocationModule : ILocationModule
    {
        private readonly ApplicationDbContext _context;

        public LocationModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBaseRepository<Governorate> Governorate =>
            _governorate ??= new BaseRepository<Governorate>(_context);
        private IBaseRepository<Governorate> _governorate;

        public IBaseRepository<Area> Area => _area ??= new BaseRepository<Area>(_context);
        private IBaseRepository<Area> _area;
    }
}
