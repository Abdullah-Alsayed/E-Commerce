using System;
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

        // Replace private fields with Lazy<T>
        private readonly Lazy<IUserRepository> _user;
        private readonly Lazy<IRoleRepository> _role;

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

            // Initialize Lazy<T> instances
            _user = new Lazy<IUserRepository>(
                () =>
                    new UserRepository(
                        _context,
                        _localizer,
                        _userManager,
                        _signInManager,
                        _roleManager,
                        _mailServices,
                        _jwt
                    )
            );
            _role = new Lazy<IRoleRepository>(
                () => new RoleRepository(_context, _roleManager, _userManager)
            );
        }

        // Update properties to use Lazy<T>.Value
        public IUserRepository User => _user.Value;
        public IRoleRepository Role => _role.Value;
    }
}
