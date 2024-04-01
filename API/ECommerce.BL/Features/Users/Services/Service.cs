using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Users.Services
{
    public class UserService : IUserService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer<UserService> _localizer;

        private string _userId = Constants.System;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public UserService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<UserService> localizer,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _httpContext = httpContextAccessor;

            #region initilize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<User, CreateUserRequest>().ReverseMap();
                cfg.CreateMap<User, UpdateUserRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.ID)
                ?.Value;

            _userName = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.FullName)
                ?.Value;

            _lang =
                _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
                ?? Languages.Ar;
            #endregion
        }

        public async Task<BaseResponse> RegisterAsync(CreateUserRequest request)
        {
            try
            {
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
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Register,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.LoginAsync(request);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Login, EntitiesEnum.User);
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UserInfoAsync()
        {
            var result = _unitOfWork.User.IsAuthenticated(_httpContext.HttpContext.User);
            List<string> userInfo = new() { _userName, _userId, result.ToString() };
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Result = userInfo,
                Message = _localizer[MessageKeys.Success].ToString()
            };
        }

        public async Task<BaseResponse> LogOfAsync()
        {
            try
            {
                await _unitOfWork.User.LogOffAsync();
                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString()
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.LogOff,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        #region helpers
        private async Task SendNotification(OperationTypeEnum action) =>
            _ = await _unitOfWork.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Entity = EntitiesEnum.User
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.User
                }
            );

        #endregion
    }
}
