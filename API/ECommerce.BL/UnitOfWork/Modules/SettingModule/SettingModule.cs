using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.SettingModule
{
    public class SettingModule : ISettingModule
    {
        private readonly ApplicationDbContext _context;

        public SettingModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBaseRepository<Setting> Setting =>
            _setting ??= new BaseRepository<Setting>(_context);
        private IBaseRepository<Setting> _setting;

        public IBaseRepository<Slider> Slider => _slider ??= new BaseRepository<Slider>(_context);
        private IBaseRepository<Slider> _slider;

        public IBaseRepository<Expense> Expense =>
            _expense ??= new BaseRepository<Expense>(_context);
        private IBaseRepository<Expense> _expense;
    }
}
