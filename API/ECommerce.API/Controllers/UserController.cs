using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service) => _service = service;

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse> Login(LoginRequest request)
        {
            try
            {
                return await _service.LoginAsync(request);
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
        public async Task<BaseResponse> DeleteUser([FromHeader] DeleteUserRequest request)
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
