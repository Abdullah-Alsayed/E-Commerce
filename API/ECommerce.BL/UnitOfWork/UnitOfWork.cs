// Ignore Spelling: Governorate

using System;
using System.Threading.Tasks;
using ECommerce.BLL.Repository;
using ECommerce.BLL.UnitOfWork.Modules.ContentModule;
using ECommerce.BLL.UnitOfWork.Modules.LocationModule;
using ECommerce.BLL.UnitOfWork.Modules.OrderModule;
using ECommerce.BLL.UnitOfWork.Modules.ProductModule;
using ECommerce.BLL.UnitOfWork.Modules.SettingModule;
using ECommerce.BLL.UnitOfWork.Modules.StockModule;
using ECommerce.BLL.UnitOfWork.Modules.UserModule;
using ECommerce.Core.Helpers;
using ECommerce.Core.Services.MailServices;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ECommerce.BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserRepository> _localizer;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMailServices _mailServices;
        private readonly JWTHelpers _jwt;

        private readonly Lazy<IProductModule> _productModule;
        private readonly Lazy<IOrderModule> _orderModule;
        private readonly Lazy<IStockModule> _stockModule;
        private readonly Lazy<IUserModule> _userModule;
        private readonly Lazy<ISettingModule> _settingModule;
        private readonly Lazy<ILocationModule> _locationModule;
        private readonly Lazy<IContentModule> _contentModule;

        public UnitOfWork(
            ApplicationDbContext context,
            IStringLocalizer<UserRepository> localizer,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IMailServices mailServices,
            IOptions<JWTHelpers> jwtOptions
        )
        {
            _context = context;
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _jwt = jwtOptions.Value;

            _productModule = new Lazy<IProductModule>(() => new ProductModule(_context));
            _orderModule = new Lazy<IOrderModule>(() => new OrderModule(_context));
            _stockModule = new Lazy<IStockModule>(() => new StockModule(_context));
            _userModule = new Lazy<IUserModule>(() => new UserModule(
                _context,
                _localizer,
                _userManager,
                _signInManager,
                _roleManager,
                _mailServices,
                _jwt
            ));
            _settingModule = new Lazy<ISettingModule>(() => new SettingModule(_context));
            _locationModule = new Lazy<ILocationModule>(() => new LocationModule(_context));
            _contentModule = new Lazy<IContentModule>(() => new ContentModule(_context));
        }

        public ApplicationDbContext Context => _context;

        public IProductModule ProductModule => _productModule.Value;
        public IOrderModule OrderModule => _orderModule.Value;
        public IStockModule StockModule => _stockModule.Value;
        public IUserModule UserModule => _userModule.Value;
        public ISettingModule SettingModule => _settingModule.Value;
        public ILocationModule LocationModule => _locationModule.Value;
        public IContentModule ContentModule => _contentModule.Value;

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public async Task<bool> IsDone(int modifyRows) => modifyRows == await SaveAsync();

        public void Dispose() => _context.Dispose();

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    }
}
