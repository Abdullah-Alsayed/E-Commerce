using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.AvatarService;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Users.Services
{
    public class UserService : IUserService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IUserContext _userContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IValidator<CreateUserRequest> _validator;

        public UserService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<UserService> localizer,
            IUserContext userContext,
            IHttpContextAccessor httpContext,
            IValidator<CreateUserRequest> validator
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userContext = userContext;
            _httpContext = httpContext;
            _validator = validator;
            #region initilize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Role, RoleDto>().ReverseMap();
                cfg.CreateMap<User, CreateUserRequest>().ReverseMap();
                cfg.CreateMap<User, UpdateUserRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper
        }

        public async Task<BaseResponse> RegisterAsync(CreateUserRequest request)
        {
            try
            {
                var user = _mapper.Map<User>(request);
                user.Language = Constants.Languages.Ar;
                user.CreateBy = string.IsNullOrEmpty(_userContext.UserId.Value)
                    ? Constants.System
                    : _userContext.UserId.Value;
                if (string.IsNullOrEmpty(request.RoleId))
                {
                    var role = await _unitOfWork.Role.FindByName(Constants.Roles.Client);
                    user.RoleId = role?.Id ?? Guid.Empty.ToString();
                }
                else
                    user.RoleId = request.RoleId;

                var result = await _unitOfWork.User.RegisterUserAsync(
                    user,
                    request.Password,
                    _userContext.UserId.Value
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
                //var validator = await _validator.ValidateAsync(request);

                //if (validator.Errors.Any())
                //    return new BaseResponse
                //    {
                //        IsSuccess = false,
                //        Message = string.Join(",", validator.Errors)
                //    };

                var user = _mapper.Map<User>(request);
                user.Email = request.Email.ToLower();
                user.Language = Constants.Languages.Ar;
                if (string.IsNullOrEmpty(request.RoleId))
                {
                    var role = await _unitOfWork.Role.FindByName(Constants.Roles.Client);
                    user.RoleId = role?.Id ?? Guid.Empty.ToString();
                }
                else
                    user.RoleId = request.RoleId;

                user.CreateBy = string.IsNullOrEmpty(_userContext.UserId.Value)
                    ? Constants.System
                    : _userContext.UserId.Value;

                var result = await _unitOfWork.User.CreateUserAsync(
                    user,
                    request.Password,
                    _userContext.UserId.Value
                );

                if (result.IsSuccess && !string.IsNullOrEmpty(user.Id))
                    await _unitOfWork.Role.AddUserToRoleAsync(
                        new Roles.Requests.AddUserToRoleRequest
                        {
                            UserID = user.Id,
                            RoleIDs = new List<string> { user.RoleId }
                        }
                    );
                if (result.IsSuccess)
                {
                    if (request.ProfilePicture != null)
                        user.Photo = await _unitOfWork.User.UploadPhotoAsync(
                            request.ProfilePicture,
                            Constants.PhotoFolder.User
                        );
                    else
                    {
                        var file = await AvatarService.GetAvatarAsFormFileAsync(
                            $"{request.FirstName} {request.LastName}"
                        );
                        var url = await _unitOfWork.User.UploadPhotoAsync(
                            file,
                            Constants.PhotoFolder.User
                        );

                        user.Photo = url;
                    }
                    await _unitOfWork.SaveAsync();
                }
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
                User.DeletedBy = _userContext.UserId.Value;
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

        public async Task<BaseResponse> WebLoginAsync(LoginRequest request, HttpContext httpContext)
        {
            try
            {
                var result = await _unitOfWork.User.WebLoginAsync(request, httpContext);
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
            List<string> userInfo =
                new() { _userContext.UserName.Value, _userContext.UserId.Value, result.ToString() };
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
                var result = await _unitOfWork.User.ChangePassword(
                    request,
                    _userContext.UserId.Value
                );
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
                var result = await _unitOfWork.User.ForgotPassword(
                    request,
                    _userContext.UserId.Value
                );
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
                var result = await _unitOfWork.User.ResetPassword(
                    request,
                    _userContext.UserId.Value
                );
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
                var user = await _unitOfWork.User.FindUserByIDAsync(_userContext.UserId.Value);
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

        public async Task<BaseResponse<BaseGridResponse<List<UserDto>>>> GetAllAsync(
            GetAllUserRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(User.CreateAt)
                    : request.SearchBy;

                var users = await _unitOfWork.User.GetAllAsync(request);
                var response = _mapper.Map<List<UserDto>>(users);
                return new BaseResponse<BaseGridResponse<List<UserDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<UserDto>>
                    {
                        Items = response,
                        Total = response.Count
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
                return new BaseResponse<BaseGridResponse<List<UserDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.User.GetAsync(userId);
                var response = _mapper.Map<UserDto>(user);
                return new BaseResponse<UserDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = response
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Get, EntitiesEnum.User);
                return new BaseResponse<BaseGridResponse<List<UserDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateLanguage(
            string language,
            HttpContext httpContext,
            HttpResponse response
        )
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var user = await _unitOfWork.User.FindUserByIDAsync(_userContext.UserId.Value);
                user.Language = language;
                var result = _mapper.Map<UserDto>(user);
                modifyRows++;

                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    await UpdateLanguageOnCookie(language, httpContext, response);
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
                    OperationTypeEnum.UpdateLanguage,
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
                    CreateBy = _userContext.UserId.Value,
                    CreateName = _userContext.UserName.Value,
                    OperationType = action,
                    Entity = EntitiesEnum.User
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userContext.UserId.Value,
                    Action = action,
                    Entity = EntitiesEnum.User
                }
            );

        private static async Task UpdateLanguageOnCookie(
            string language,
            HttpContext httpContext,
            HttpResponse response
        )
        {
            var currentUser = httpContext.User;
            var claimsIdentity = currentUser.Identity as ClaimsIdentity;

            // Update the claims with the new language
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("Language"));

            claimsIdentity.AddClaim(new Claim("Language", language ?? ""));

            // Update the authentication cookie with the new claims
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { AllowRefresh = true }
            );
            response.Cookies.Append(
                "PreferredLanguage",
                language,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }

        public async Task SeedData()
        {
            await _unitOfWork.User.SeedData();
        }

        #endregion
    }
}
