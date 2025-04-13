using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.Governorates.Dtos;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Governorates.Services;

public class GovernorateService : IGovernorateService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<GovernorateService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public GovernorateService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GovernorateService> localizer,
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
            cfg.CreateMap<Governorate, GovernorateDto>()
                .ForMember(
                    dest => dest.Name,
                    opt =>
                        opt.MapFrom(src =>
                            _lang == Constants.Languages.Ar ? src.NameAR : src.NameEN
                        )
                )
                .ReverseMap();
            cfg.CreateMap<Governorate, CreateGovernorateRequest>().ReverseMap();
            cfg.CreateMap<Governorate, UpdateGovernorateRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindGovernorateRequest request)
    {
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            var result = _mapper.Map<GovernorateDto>(governorate);
            return new BaseResponse<GovernorateDto>
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
                OperationTypeEnum.View,
                EntitiesEnum.Governorate
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse<BaseGridResponse<List<GovernorateDto>>>> GetAllAsync(
        GetAllGovernorateRequest request
    )
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? _lang == Languages.Ar
                    ? nameof(Governorate.NameAR)
                    : nameof(Governorate.NameEN)
                : request.SearchBy;

            var result = await _unitOfWork.Governorate.GetAllAsync(request);
            var response = _mapper.Map<List<GovernorateDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<GovernorateDto>>
                {
                    Items = response,
                    Total = response != null ? result.count : 0,
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.GetAll,
                EntitiesEnum.Governorate
            );
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateGovernorateRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var governorate = _mapper.Map<Governorate>(request);
            governorate = await _unitOfWork.Governorate.AddAsync(governorate, _userId);
            var result = _mapper.Map<GovernorateDto>(governorate);

            //#region Send Notification
            //await SendNotification(OperationTypeEnum.Create);
            //modifyRows++;
            //#endregion

            //#region Log
            //await LogHistory(OperationTypeEnum.Create);
            //modifyRows++;
            //#endregion

            modifyRows++;
            if (await _unitOfWork.IsDone(modifyRows))
            {
                await transaction.CommitAsync();
                return new BaseResponse<GovernorateDto>
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
                EntitiesEnum.Governorate
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateGovernorateRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            _mapper.Map(request, governorate);
            governorate.ModifyBy = _userId;
            governorate.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<GovernorateDto>(governorate);

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
                return new BaseResponse<GovernorateDto>
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
                EntitiesEnum.Governorate
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveGovernorateRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            governorate.ModifyBy = _userId;
            governorate.ModifyAt = DateTime.UtcNow;
            governorate.IsActive = !governorate.IsActive;
            var result = _mapper.Map<GovernorateDto>(governorate);

            //#region Send Notification
            //await SendNotification(OperationTypeEnum.Toggle);
            //modifyRows++;
            //#endregion

            //#region Log
            //await LogHistory(OperationTypeEnum.Toggle);
            //modifyRows++;
            //#endregion

            modifyRows++;
            if (await _unitOfWork.IsDone(modifyRows))
            {
                await transaction.CommitAsync();
                return new BaseResponse<GovernorateDto>
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
                OperationTypeEnum.Toggle,
                EntitiesEnum.Governorate
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteGovernorateRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            governorate.DeletedBy = _userId;
            governorate.DeletedAt = DateTime.UtcNow;
            governorate.IsDeleted = true;
            var result = _mapper.Map<GovernorateDto>(governorate);

            //#region Send Notification
            //await SendNotification(OperationTypeEnum.Delete);
            //modifyRows++;
            //#endregion

            //#region Log
            //await LogHistory(OperationTypeEnum.Delete);
            //modifyRows++;
            //#endregion

            modifyRows++;
            if (await _unitOfWork.IsDone(modifyRows))
            {
                await transaction.CommitAsync();
                return new BaseResponse<GovernorateDto>
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
                EntitiesEnum.Governorate
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
            var result = _unitOfWork.Governorate.SearchEntity();
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
                EntitiesEnum.Governorate
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
                Entity = EntitiesEnum.Governorate
            }
        );

    private async Task LogHistory(OperationTypeEnum action) =>
        await _unitOfWork.History.AddAsync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Governorate
            },
            _userId
        );

    #endregion
}
