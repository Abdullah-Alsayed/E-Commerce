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
        }

        public ApplicationDbContext Context => _context;

        private IProductModule _productModule;
        public IProductModule ProductModule => _productModule ??= new ProductModule(_context);

        private IOrderModule _orderModule;
        public IOrderModule OrderModule => _orderModule ??= new OrderModule(_context);

        private IStockModule _stockModule;
        public IStockModule StockModule => _stockModule ??= new StockModule(_context);

        private IUserModule _userModule;
        public IUserModule UserModule =>
            _userModule ??= new UserModule(
                _context,
                _localizer,
                _userManager,
                _signInManager,
                _roleManager,
                _mailServices,
                _jwt
            );

        private ISettingModule _settingModule;
        public ISettingModule SettingModule => _settingModule ??= new SettingModule(_context);

        private ILocationModule _locationModule;
        public ILocationModule LocationModule => _locationModule ??= new LocationModule(_context);

        private IContentModule _contentModule;
        public IContentModule ContentModule => _contentModule ??= new ContentModule(_context);

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public async Task<bool> IsDone(int modifyRows) => modifyRows == await SaveAsync();

        public void Dispose() => _context.Dispose();

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    }
}
