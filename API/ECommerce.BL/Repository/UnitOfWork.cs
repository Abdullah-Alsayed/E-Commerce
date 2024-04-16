// Ignore Spelling: Governorate

using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Repository;
using ECommerce.Core;
using ECommerce.Core.Services.MailServices;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ECommerce.BL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserRepository> _localizer;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMailServices _mailServices;
        private readonly JWTHelpers _jwt;

        public UnitOfWork(
            ApplicationDbContext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMailServices mailServices,
            IOptions<JWTHelpers> jwt,
            IStringLocalizer<UserRepository> localizer
        )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _jwt = jwt.Value;
            _localizer = localizer;

            User = new UserRepository(
                _context,
                localizer,
                _userManager,
                _signInManager,
                _roleManager,
                _mailServices,
                _jwt
            );
            Role = new RoleRepository(_context, _roleManager, _userManager);
            Product = new ProductRepository(_context);
            SubCategory = new BaseRepository<SubCategory>(_context);
            Governorate = new BaseRepository<Governorate>(_context);
            Slider = new BaseRepository<Slider>(_context);
            Notification = new NotificationRepository(_context);
            Voucher = new BaseRepository<Voucher>(_context);
            ErrorLog = new ErrorRepository(_context);
            Invoice = new InvoiceRepository(_context);
            ProductSize = new ProductSizeRepository(_context);
            ProductColor = new ProductColorRepository(_context);
            Stock = new StockRepository(_context);
            Size = new BaseRepository<Size>(_context);
            Feedback = new BaseRepository<Feedback>(_context);
            Order = new BaseRepository<Order>(_context);
            History = new BaseRepository<History>(_context);
            Vendor = new BaseRepository<Vendor>(_context);
            ContactUs = new BaseRepository<ContactUs>(_context);
            Category = new BaseRepository<Category>(_context);
            Expense = new BaseRepository<Expense>(_context);
            Setting = new BaseRepository<Setting>(_context);
            Status = new BaseRepository<Status>(_context);
            Review = new BaseRepository<ProductReview>(_context);
            Cart = new BaseRepository<ShoppingCart>(_context);
            Color = new BaseRepository<Color>(_context);
            Brand = new BaseRepository<Brand>(_context);
            Unit = new BaseRepository<Unit>(_context);
            Area = new BaseRepository<Area>(_context);
            Booking = new BaseRepository<Booking>(_context);
            Context = _context;
        }

        public IBaseRepository<SubCategory> SubCategory { get; private set; }
        public IBaseRepository<Governorate> Governorate { get; private set; }
        public IBaseRepository<Slider> Slider { get; private set; }
        public IBaseRepository<Order> Order { get; private set; }
        public IBaseRepository<Voucher> Voucher { get; private set; }
        public IBaseRepository<ContactUs> ContactUs { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IStockRepository Stock { get; private set; }
        public IBaseRepository<Size> Size { get; private set; }
        public IErrorRepository ErrorLog { get; private set; }
        public IBaseRepository<History> History { get; private set; }
        public IInvoiceRepository Invoice { get; private set; }
        public IBaseRepository<Vendor> Vendor { get; private set; }
        public IBaseRepository<Feedback> Feedback { get; private set; }
        public IBaseRepository<Category> Category { get; private set; }
        public IBaseRepository<Expense> Expense { get; private set; }
        public IBaseRepository<Setting> Setting { get; private set; }
        public IBaseRepository<ShoppingCart> Cart { get; private set; }
        public IBaseRepository<ProductReview> Review { get; private set; }
        public IBaseRepository<Status> Status { get; private set; }
        public IBaseRepository<Color> Color { get; private set; }
        public IBaseRepository<Brand> Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public IBaseRepository<Unit> Unit { get; private set; }
        public IBaseRepository<Area> Area { get; private set; }
        public IUserRepository User { get; private set; }
        public IRoleRepository Role { get; private set; }
        public IProductSizeRepository ProductSize { get; private set; }
        public IProductColorRepository ProductColor { get; private set; }
        public IBaseRepository<Booking> Booking { get; private set; }

        public ApplicationDbContext Context { get; set; }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public async Task<bool> IsDone(int modifyRows)
        {
            var count = await SaveAsync();
            return count == modifyRows;
        }

        public void Dispose() => _context.Dispose();
    }
}
