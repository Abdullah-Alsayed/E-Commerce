using System.Security.Claims;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Account.Dtos;
using ECommerce.BLL.Features.Account.Requests;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.BLL.IRepository
{
    public interface IUserRepository
    {
        Task<BaseResponse> LoginAsync(LoginRequest request);
        Task<BaseResponse<CreateUserDto>> CreateUserAsync(
            User user,
            string password,
            string userId
        );
        Task SendConfirmEmailAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User User, string Toke);
        Task ForgotPasswordAsync(object Entity);
        Task ResetPasswordAsync(string Code);
        Task UpdatePasswordAsync(string username, string password);
        Task<User> FindUserByIDAsync(string UserID);
        Task<User> FindUserByNameAsync(string Name);
        Task<User> FindUserByEmailAsync(string Email);
        Task<bool> EmailExisteAsync(string email);
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
