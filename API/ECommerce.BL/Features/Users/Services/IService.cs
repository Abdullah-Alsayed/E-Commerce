using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Users.Services
{
    public interface IUserService
    {
        Task<BaseResponse> ChangePasswordAsync(ChangePasswordUserRequest request);
        Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailUserRequest request);
        Task<BaseResponse> CreateAsync(CreateUserRequest request);
        Task<BaseResponse> UpdateAsync(UpdateUserRequest request);
        Task<BaseResponse> DeleteAsync(DeleteUserRequest request);
        Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordUserRequest request);
        Task<BaseResponse> LoginAsync(LoginRequest request);
        Task<BaseResponse> WebLoginAsync(LoginRequest request, HttpContext httpContext);
        Task<BaseResponse> LogOfAsync();

        //Task<BaseResponse> FindAsync(FindUserRequest request);
        Task<BaseResponse<BaseGridResponse<List<UserDto>>>> GetAllAsync(GetAllUserRequest request);

        //Task<BaseResponse> UpdateAsync(UpdateUserRequest request);
        //Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ToggleActive(BaseRequest request);
        Task<BaseResponse> RegisterAsync(CreateUserRequest request);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordUserRequest request);
        Task<BaseResponse> SendConfirmEmailAsync();
        Task<BaseResponse> UserInfoAsync();
        Task<BaseResponse> ChangeUserPasswordAsync(ChangeUserPasswordRequest request);
        Task<BaseResponse> UpdateLanguage(
            string language,
            HttpContext httpContext,
            HttpResponse response
        );
        Task SeedData();
        Task<BaseResponse> GetAsync(Guid userId);
    }
}
