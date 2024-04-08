using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using ECommerce.BLL.Features.Units.Dtos;
using ECommerce.BLL.Features.Units.Requests;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
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

        public async Task<BaseResponse> CreateAsync(CreateUserRequest request)
        {
            try
            {
                var user = _mapper.Map<User>(request);
                user.UserName = request.UserName.ToLower();
                user.Email = request.Email.ToLower();
                user.Language = Constants.Languages.Ar;
                user.CreateBy = string.IsNullOrEmpty(_userId) ? Constants.System : _userId;
                var result = await _unitOfWork.User.CreateUserAsync(
                    user,
                    request.Password,
                    _userId,
                    Constants.Roles.User
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

        public async Task<BaseResponse> DeleteAsync(DeleteUserRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var User = await _unitOfWork.User.DeleteAsync(request.ID.ToString(), request.Token);
                User.DeletedBy = _userId;
                User.DeletedAt = DateTime.UtcNow;
                User.IsDeleted = true;
                var result = _mapper.Map<UserDto>(User);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

                modifyRows += 2;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<UserDto>
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
                        Result = result
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = _localizer[MessageKeys.Fail].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Delete,
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

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordUserRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.ChangePassword(request, _userId);
                return new BaseResponse
                {
                    IsSuccess = result.IsSuccess,
                    Message = _localizer[result.Message].ToString()
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.ChangePassword,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ChangeUserPasswordAsync(ChangeUserPasswordRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.ChangeUserPassword(request);
                return new BaseResponse
                {
                    IsSuccess = result.IsSuccess,
                    Message = _localizer[result.Message].ToString()
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.ChangeUserPassword,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordUserRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.ForgotPassword(request, _userId);
                return new BaseResponse { IsSuccess = result.IsSuccess, Message = result.Message };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.ForgotPassword,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordUserRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.ResetPassword(request, _userId);
                return new BaseResponse { IsSuccess = result.IsSuccess, Message = result.Message };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.ResetPassword,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ConfirmEmailAsync(ConfirmEmailUserRequest request)
        {
            try
            {
                var result = await _unitOfWork.User.ConfirmEmailAsync(request.ID, request.Token);
                return new BaseResponse { IsSuccess = result.IsSuccess, Message = result.Message };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.ResetPassword,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> SendConfirmEmailAsync()
        {
            try
            {
                var user = await _unitOfWork.User.FindUserByIDAsync(_userId);
                var isConfirm = await _unitOfWork.User.IsConfirmedAsync(user);
                if (!isConfirm)
                {
                    var result = await _unitOfWork.User.SendConfirmEmailAsync(user);
                    return new BaseResponse
                    {
                        IsSuccess = result,
                        Message = result
                            ? _localizer[Constants.MessageKeys.Success].ToString()
                            : _localizer[Constants.MessageKeys.Fail].ToString()
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = _localizer[Constants.MessageKeys.EmailIsConfirm].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.SendConfirmEmail,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetAllAsync(GetAllUserRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(User.UserName)
                    : request.SearchBy;

                var Users = await _unitOfWork.User.GetAllAsync(request);
                var response = _mapper.Map<List<UserDto>>(Users);
                return new BaseResponse<BaseGridResponse<List<UserDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<UserDto>>
                    {
                        Items = response,
                        Total = response != null ? response.Count : 0
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.GetAll,
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
