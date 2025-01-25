using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace ECommerce.BLL.IRepository
{
    public interface IUserRepository
    {
        Task<BaseResponse> LoginAsync(LoginRequest request);
        Task<BaseResponse<CreateUserDto>> CreateUserAsync(
            User user,
            string password,
            string userId,
            string Role = null
        );
        Task<bool> SendConfirmEmailAsync(User user);
        Task<BaseResponse> ConfirmEmailAsync(string userID, string toke);
        Task ForgotPasswordAsync(object Entity);
        Task ResetPasswordAsync(string Code);
        Task UpdatePasswordAsync(string username, string password);
        Task<User> FindUserByIDAsync(string UserID);
        Task<User> FindUserByNameAsync(string Name);
        Task<User> FindUserByEmailAsync(string Email);
        Task<bool> EmailExistAsync(string email);
        bool PhoneExist(string PhoneNumber);
        Task<bool> UserNameExistAsync(string userName);
        Task<bool> IsConfirmedAsync(User user);
        string GetUserID(ClaimsPrincipal user);
        string GetUserName(ClaimsPrincipal user);
        string GetRoleID(ClaimsPrincipal user);
        bool IsAuthenticated(ClaimsPrincipal user);
        Task LoginAsync(User user, bool RememberMe);
        Task<BaseResponse> WebLoginAsync(LoginRequest request, HttpContext httpContext);
        Task LogOffAsync();
        Task<BaseResponse> ChangePassword(ChangePasswordUserRequest request, string userId);
        Task<BaseResponse> ForgotPassword(ForgotPasswordUserRequest request, string userId);
        Task<BaseResponse> ResetPassword(ResetPasswordUserRequest request, string userId);
        Task<User> DeleteAsync(string ID, string token);
        bool IsTokenExperts(StringValues token);
        Task<List<User>> GetAllAsync(GetAllUserRequest request);
        Task<BaseResponse> ChangeUserPassword(ChangeUserPasswordRequest request);
    }
}
