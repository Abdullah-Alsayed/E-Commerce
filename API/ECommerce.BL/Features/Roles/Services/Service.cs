using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;
using static ECommerce.Core.PermissionsClaims.Permissions;
using Role = ECommerce.DAL.Entity.Role;

namespace ECommerce.BLL.Features.Roles.Services
{
    public class RoleService : IRoleService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<RoleService> _localizer;
        private readonly IUserContext _userContext;

        readonly Guid _userId;

        public RoleService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<RoleService> localizer,
            IUserContext userContext
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userContext = userContext;

            #region initilize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Role, RoleDto>()
                    .ForMember(
                        dest => dest.Name,
                        opt =>
                            opt.MapFrom(src =>
                                _userContext.Language.Value == Constants.Languages.Ar
                                    ? src.Name
                                    : src.NameEn
                            )
                    )
                    .ReverseMap();
                cfg.CreateMap<Role, CreateRoleRequest>().ReverseMap();
                cfg.CreateMap<Role, UpdateRoleRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindRoleRequest request)
        {
            try
            {
                var Role = await _unitOfWork.Role.FirstAsync(x => x.Id == request.ID);
                var result = _mapper.Map<RoleDto>(Role);
                return new BaseResponse<RoleDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.View, EntitiesEnum.Role);
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<RoleDto>>>> GetAllAsync(
            GetAllRoleRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(Role.Name)
                    : request.SearchBy;

                var roles = await _unitOfWork.Role.GetAllAsync(request);
                var response = _mapper.Map<List<RoleDto>>(roles);
                return new BaseResponse<BaseGridResponse<List<RoleDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<RoleDto>>
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
                    EntitiesEnum.Role
                );
                return new BaseResponse<BaseGridResponse<List<RoleDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateRoleRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var role = _mapper.Map<Role>(request);

                role = await _unitOfWork.Role.AddAsync(role, _userContext.UserId.Value);
                role.NormalizedName = request.Name.ToUpper();

                var result = _mapper.Map<RoleDto>(role);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Create);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Create);
                modifyRows++;
                #endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<RoleDto>
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
                    OperationTypeEnum.Create,
                    EntitiesEnum.Role
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateRoleRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var role = await _unitOfWork.Role.FirstAsync(x => x.Id == request.ID);
                _mapper.Map(request, role);
                role.NormalizedName = request.Name.ToUpper();
                _unitOfWork.Role.Update(role, _userContext.UserId.Value);
                var result = _mapper.Map<RoleDto>(role);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.Update);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.Update);
                //modifyRows++;
                //#endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<RoleDto>
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
                    OperationTypeEnum.Update,
                    EntitiesEnum.Role
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteRoleRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var role = await _unitOfWork.Role.FirstAsync(x => x.Id == request.ID);
                _unitOfWork.Role.Delete(role, _userContext.UserId.Value);
                var result = _mapper.Map<RoleDto>(role);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<RoleDto>
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
                    EntitiesEnum.Role
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.Role.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Search,
                    EntitiesEnum.Role
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateRoleClaimsAsync(UpdateClaimsRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                modifyRows = await _unitOfWork.Role.UpdateRoleClaimsAsync(request);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.UpdateClaims);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.UpdateClaims);
                //modifyRows++;
                //#endregion

                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
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
                    OperationTypeEnum.UpdateClaims,
                    EntitiesEnum.Role
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateUserClaimsAsync(UpdateUserClaimsRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                modifyRows = await _unitOfWork.Role.UpdateUserClaimsAsync(request);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.UpdateClaims);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.UpdateClaims);
                //modifyRows++;
                //#endregion

                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
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
                    OperationTypeEnum.UpdateClaims,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> AddUserToRoleAsync(AddUserToRoleRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var result = await _unitOfWork.Role.AddUserToRoleAsync(request);
                #region Send Notification
                await SendNotification(OperationTypeEnum.AddUserInRole);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.AddUserInRole);
                modifyRows++;
                #endregion

                if (await _unitOfWork.IsDone(modifyRows) && result)
                {
                    await transaction.CommitAsync();
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
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
                    OperationTypeEnum.AddUserInRole,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateUserRoleAsync(UpdateUserRoleRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var result = await _unitOfWork.Role.UpdateUserRoleAsync(request);
                #region Send Notification
                await SendNotification(OperationTypeEnum.AddUserInRole);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.AddUserInRole);
                modifyRows++;
                #endregion

                if (await _unitOfWork.IsDone(modifyRows) && result)
                {
                    await transaction.CommitAsync();
                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
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
                    OperationTypeEnum.AddUserInRole,
                    EntitiesEnum.User
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<List<AllClaimsDto>>> GetClaimsAsync(BaseRequest request)
        {
            try
            {
                var roleClaims = await _unitOfWork.Role.GetRoleClaims(request.ID);
                var allPermissions = Permissions.GetAllPermissions();
                var claims = allPermissions
                    .GroupBy(x => x.Module)
                    .Select(claims => new AllClaimsDto
                    {
                        Key = claims.Key,
                        ID = request.ID.ToString(),
                        Claims = claims
                            .Select(claim => new ClaimDto
                            {
                                IsChecked = roleClaims.Exists(x => x == claim.Claim),
                                Claim = claim.Claim,
                                Module = claim.Module,
                                Operation = claim.Operation,
                            })
                            .ToList()
                    })
                    .ToList();

                return new BaseResponse<List<AllClaimsDto>>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success],
                    Count = claims.Count,
                    Result = claims
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.UpdateClaims,
                    EntitiesEnum.Role
                );
                return new BaseResponse<List<AllClaimsDto>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<List<AllClaimsDto>>> GetUserClaimsAsync(BaseRequest request)
        {
            try
            {
                var userId = await _unitOfWork.User.GetAsync(request.ID);
                var userClaims = await _unitOfWork.Role.GetUserClaims(request.ID);
                var roleClaims = await _unitOfWork.Role.GetRoleClaims(userId.RoleId);
                var allClaims = userClaims.Union(roleClaims).Distinct().ToList();
                var allPermissions = Permissions.GetAllPermissions();
                var claims = allPermissions
                    .GroupBy(x => x.Module)
                    .Select(claims => new AllClaimsDto
                    {
                        Key = claims.Key,
                        ID = request.ID.ToString(),
                        Claims = claims
                            .Select(claim => new ClaimDto
                            {
                                IsChecked = allClaims.Exists(x => x == claim.Claim),
                                Claim = claim.Claim,
                                Module = claim.Module,
                                Operation = claim.Operation,
                            })
                            .ToList()
                    })
                    .ToList();

                return new BaseResponse<List<AllClaimsDto>>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success],
                    Count = claims.Count,
                    Result = claims
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.UpdateClaims,
                    EntitiesEnum.Role
                );
                return new BaseResponse<List<AllClaimsDto>>
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
                    Entity = EntitiesEnum.Role
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userContext.UserId.Value,
                    Action = action,
                    Entity = EntitiesEnum.Role
                },
                _userId
            );

        #endregion
    }
}
