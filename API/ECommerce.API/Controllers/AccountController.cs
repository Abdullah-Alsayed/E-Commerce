using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Account.Dtos;
using ECommerce.BLL.Features.Account.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        private string _userId = string.Empty;

        public AccountController(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContext,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _userId = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<BaseResponse<CreateUserDto>> RegisterAsync(CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BaseResponse<CreateUserDto>
                    {
                        IsSuccess = false,
                        Message = Constants.Errors.Register
                    };

                var user = _mapper.Map<User>(request);
                user.Language = Constants.Languages.Ar;
                user.CreateBy = string.IsNullOrEmpty(_userId) ? Constants.System : _userId;

                var result = await _unitOfWork.User.CreateUserAsync(
                    user,
                    request.Password,
                    _userId
                );
                return new BaseResponse<CreateUserDto>
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    Result = result.Result
                };
            }
            catch (Exception ex)
            {
                await ErrorLog(ex);
                return new BaseResponse<CreateUserDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<BaseResponse> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = Constants.Errors.LoginFiled
                    };

                var result = await _unitOfWork.User.LoginAsync(request);
                return result;
            }
            catch (Exception ex)
            {
                await ErrorLog(ex);
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("Userinfo")]
        public IActionResult Userinfo()
        {
            string UserID = _unitOfWork.User.GetUserID(User);
            string UserName = _unitOfWork.User.GetUserName(User);
            _ = _unitOfWork.User.IsAuthenticated(User);
            List<string> InfoList = new() { UserName, UserID };
            return Ok(InfoList);
        }

        [HttpGet]
        [Route("Logof")]
        public async Task<IActionResult> Logof()
        {
            await _unitOfWork.User.LogOffAsync();
            return Ok();
        }

        #region helpers
        private async Task ErrorLog(Exception ex)
        {
            await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Message = ex.Message, Source = ex.Source, }
            );
            await _unitOfWork.SaveAsync();
        }
        #endregion
    }
}
