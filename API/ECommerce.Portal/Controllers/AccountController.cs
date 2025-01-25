using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;

        public AccountController(IUserService service) => _service = service;

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse> Register(CreateUserRequest request)
        {
            try
            {
                return await _service.RegisterAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            // Validate the ReturnUrl to prevent open redirect vulnerabilities
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            ViewData["ReturnUrl"] = returnUrl; // Pass the ReturnUrl to the view
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse> Login(LoginRequest request)
        {
            try
            {
                return await _service.WebLoginAsync(request, HttpContext);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ChangePassword([FromForm] ChangePasswordUserRequest request)
        {
            try
            {
                return await _service.ChangePasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ChangeUserPassword(
            [FromForm] ChangeUserPasswordRequest request
        )
        {
            try
            {
                return await _service.ChangeUserPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ForgotPassword([FromForm] ForgotPasswordUserRequest request)
        {
            try
            {
                return await _service.ForgotPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ResetPassword([FromForm] ResetPasswordUserRequest request)
        {
            try
            {
                return await _service.ResetPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ConfirmEmail([FromForm] ConfirmEmailUserRequest request)
        {
            try
            {
                return await _service.ConfirmEmailAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> SendConfirmEmail()
        {
            try
            {
                return await _service.SendConfirmEmailAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<BaseResponse> CreateUser([FromForm] CreateUserRequest request)
        {
            try
            {
                return await _service.CreateAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllUserRequest request)
        {
            try
            {
                return await _service.GetAllAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpDelete]
        public async Task<BaseResponse> DeleteUser(DeleteUserRequest request)
        {
            try
            {
                return await _service.DeleteAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> UserInfo()
        {
            try
            {
                return await _service.UserInfoAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<BaseResponse> LogOfAsync()
        {
            try
            {
                return await _service.LogOfAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
