using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services.MailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Applicationdbcontext _context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMailServices mailServices;

        public UserRepository(Applicationdbcontext context, UserManager<User> userManager,
                              SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,
                               IMailServices mailServices)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.mailServices = mailServices;
        }

        public async Task<User> FindUserByIDAsync(string UserID)
        {
            return await userManager.FindByIdAsync(UserID);
        }
        public string GetUserID(ClaimsPrincipal user)
        {
            //return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             return userManager.GetUserId(user);
        }
        public string GetUserName(ClaimsPrincipal user)
        {
            return userManager.GetUserName(user);
        }
        public string GetRoleID(ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == "RoleID");
            if (claim != null)
                return claim.Value;
            return null;
        }
        public async Task<User> FindUserByNameAsync(string Name)
        {
            return await userManager.FindByNameAsync(Name);
        }
        public async Task<User> FindUserByEmailAsync(string Email)
        {
            var Result =  await userManager.FindByEmailAsync(Email);
            return Result;
        }
        public async Task SendConfirmEmailAsync(User user)
        {
            var Token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var EncodingToken = Encoding.UTF8.GetBytes(Token);
            var newToken = WebEncoders.Base64UrlEncode(EncodingToken);

            var confirmLink = $"https://localhost:44392/Account/ConfirmEmail?ID={user.Id}&Tocken={newToken}";
            var txt = "Please confirm your registration at our sute";
            var link = "<a href=\"" + confirmLink + "\">Confirm registration</a>";
            var subject = "Registration Confirm";
           //mailServices.SendMail(user.Email, user.FitsrName, txt, link, subject);

        }
        public Task ForgotPasswordAsync(object Entity)
        {
            throw new NotImplementedException();
        }
        public async Task<SignInResult> LoginAsync(string UserName, string Password, bool RememberMe)
        {
            var Rusult = await signInManager.PasswordSignInAsync(UserName, Password, RememberMe, false);
            return Rusult;
        }
        public async Task LoginAsync(User user, bool RememberMe)
        {
            await signInManager.SignInAsync(user,RememberMe);
        }
        public async Task LogOffAsync()
        {
            await signInManager.SignOutAsync();
        }
        public Task ResetPasswordAsync(string Code)
        {
            throw new NotImplementedException();
        }
        public Task UpdatePasswordAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
        public async Task<IdentityResult> RegisterAsync(User user, string Password)
        {
            var Result = await userManager.CreateAsync(user, Password);
            return Result;
        }
        public async Task<bool> EmailExistesAsync(string email)
        {
            var Result = await FindUserByEmailAsync(email) == null ? true : false;
            return Result;
        }
        public async Task<bool> UserNameExistesAsync(string userName)
        {
            var Result = await FindUserByNameAsync(userName) == null ? true : false;
            return Result;
        }
        public async Task<IdentityResult> ConfirmEmailAsync(User User, string Token)
        {
            var newToken = WebEncoders.Base64UrlDecode(Token);
            var encodeToken = Encoding.UTF8.GetString(newToken);
            return await userManager.ConfirmEmailAsync(User, encodeToken);
        }
        public async Task<bool> IsConfirmedAsync(User user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }
        public bool IsAuthenticated(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public bool PhoneExistes(string PhoneNumber)
        {
          var Result =_context.Users.Where(p => p.PhoneNumber == PhoneNumber).Any();
            return !Result;
        }
    }
}
