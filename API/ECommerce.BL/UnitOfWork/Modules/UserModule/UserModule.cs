using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.Core.Helpers;
using ECommerce.Core.Services.MailServices;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace ECommerce.BLL.UnitOfWork.Modules.UserModule
{
    public class UserModule : IUserModule
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserRepository> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMailServices _mailServices;
        private readonly JWTHelpers _jwt;

        public UserModule(
            ApplicationDbContext context,
            IStringLocalizer<UserRepository> localizer,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IMailServices mailServices,
            JWTHelpers jwt
        )
        {
            _context = context;
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _jwt = jwt;
        }

        public IUserRepository User =>
            _user ??= new UserRepository(
                _context,
                _localizer,
                _userManager,
                _signInManager,
                _roleManager,
                _mailServices,
                _jwt
            );
        private IUserRepository _user;

        public IRoleRepository Role =>
            _role ??= new RoleRepository(_context, _roleManager, _userManager);
        private IRoleRepository _role;
    }
}
