using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IUserRepository
    {
        Task<SignInResult> LoginAsync(string UserName, string Password, bool RememberMe);
        Task<IdentityResult> RegisterAsync(User user, string Password);
        Task SendConfirmEmailAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User User, string Toke);
        Task ForgotPasswordAsync(object Entity);
        Task ResetPasswordAsync(string Code);
        Task UpdatePasswordAsync(string username, string password);
        Task<User> FindUserByIDAsync(string UserID);
        Task<User> FindUserByNameAsync(string Name);
        Task<User> FindUserByEmailAsync(string Email);
        Task<bool> EmailExistesAsync(string email);
        bool PhoneExistes(string PhoneNumber);
        Task<bool> UserNameExistesAsync(string userName);
        Task<bool> IsConfirmedAsync(User user);
        string GetUserID(ClaimsPrincipal user);
        string GetUserName(ClaimsPrincipal user);
        string GetRoleID(ClaimsPrincipal user);
        bool IsAuthenticated(ClaimsPrincipal user);
        Task LoginAsync(User user, bool RememberMe);
        Task LogOffAsync();
    }
}
