// Ignore Spelling: Governorate

using ECommerce.BLL.IRepository;
using ECommerce.BLL.Repository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using ECommerce.Helpers;
using ECommerce.Services.MailServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ECommerce.BL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly Applicationdbcontext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailServices _mailServices;
        private readonly IHostingEnvironment _hosting;
        private readonly JWTHelpers _jwt;

        public UnitOfWork(
            Applicationdbcontext context,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMailServices mailServices,
            IHostingEnvironment hosting,
            IOptions<JWTHelpers> jwt
        )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _hosting = hosting;
            _jwt = jwt.Value;

            Product = new ProductRepository(_context, _hosting);
            User = new UserRepository(
                _context,
                _userManager,
                _signInManager,
                _roleManager,
                _mailServices,
                _jwt
            );
            SubCategory = new BaseRepository<SubCategory>(_context, _hosting);
            Governorate = new BaseRepository<Governorate>(_context, _hosting);
            SliderPhoto = new BaseRepository<SliderPhoto>(_context, _hosting);
            Notification = new NotificationRepository(_context, _hosting);
            PromoCode = new BaseRepository<PromoCode>(_context, _hosting);
            ProductPhoto = new ProductPhotoRepository(_context, _hosting);
            ContactUs = new BaseRepository<ContactUs>(_context, _hosting);
            Category = new BaseRepository<Category>(_context, _hosting);
            ErrorLog = new BaseRepository<ErrorLog>(_context, _hosting);
            Expense = new BaseRepository<Expense>(_context, _hosting);
            Setting = new BaseRepository<Setting>(_context, _hosting);
            Status = new BaseRepository<Status>(_context, _hosting);
            Review = new BaseRepository<Review>(_context, _hosting);
            Color = new BaseRepository<Color>(_context, _hosting);
            Brand = new BaseRepository<Brand>(_context, _hosting);
            Unit = new BaseRepository<Unit>(_context, _hosting);
            Area = new BaseRepository<Area>(_context, _hosting);
            Context = _context;
        }

        public IBaseRepository<SubCategory> SubCategory { get; private set; }
        public IBaseRepository<Governorate> Governorate { get; private set; }
        public IBaseRepository<SliderPhoto> SliderPhoto { get; private set; }
        public IBaseRepository<PromoCode> PromoCode { get; private set; }
        public IBaseRepository<ContactUs> ContactUs { get; private set; }
        public IProductPhotoRepository ProductPhoto { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public IBaseRepository<ErrorLog> ErrorLog { get; private set; }
        public IBaseRepository<Category> Category { get; private set; }
        public IBaseRepository<Expense> Expense { get; private set; }
        public IBaseRepository<Setting> Setting { get; private set; }
        public IBaseRepository<Review> Review { get; private set; }
        public IBaseRepository<Status> Status { get; private set; }
        public IBaseRepository<Color> Color { get; private set; }
        public IBaseRepository<Brand> Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public IBaseRepository<Unit> Unit { get; private set; }
        public IBaseRepository<Area> Area { get; private set; }
        public IUserRepository User { get; private set; }

        public Applicationdbcontext Context { get; set; }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
