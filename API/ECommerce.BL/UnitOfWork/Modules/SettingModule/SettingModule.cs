using System;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.SettingModule
{
    public class SettingModule : ISettingModule
    {
        private readonly ApplicationDbContext _context;

        // All private fields using Lazy<T>
        private readonly Lazy<IBaseRepository<Setting>> _setting;
        private readonly Lazy<IBaseRepository<Slider>> _slider;
        private readonly Lazy<IBaseRepository<Expense>> _expense;

        public SettingModule(ApplicationDbContext context)
        {
            _context = context;

            // Initialize all Lazy<T> instances
            _setting = new Lazy<IBaseRepository<Setting>>(
                () => new BaseRepository<Setting>(_context)
            );
            _slider = new Lazy<IBaseRepository<Slider>>(() => new BaseRepository<Slider>(_context));
            _expense = new Lazy<IBaseRepository<Expense>>(
                () => new BaseRepository<Expense>(_context)
            );
        }

        // All properties using Lazy<T>.Value
        public IBaseRepository<Setting> Setting => _setting.Value;
        public IBaseRepository<Slider> Slider => _slider.Value;
        public IBaseRepository<Expense> Expense => _expense.Value;
    }
}
