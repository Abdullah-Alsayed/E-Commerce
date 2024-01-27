using AutoMapper;
using ECommerce.BLL.Futures.Governorates.Dtos;
using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.BLL.Futures.Governorates.Validators;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using ECommerce.Helpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ECommerce.Helpers.Constants;

namespace ECommerce.BLL.Futures.Governorates.Services;

public class GovernorateServices : IGovernorateServices
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<CreateGovernorateValidator> _localizer;

    public GovernorateServices(
        IUnitOfWork unitOfWork,
        IStringLocalizer<CreateGovernorateValidator> localizer
    )
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;

        #region initilize mapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.CreateMap<Governorate, GovernorateDto>().ReverseMap();
            cfg.CreateMap<Governorate, CreateGovernorateRequest>().ReverseMap();
            cfg.CreateMap<Governorate, UpdateGovernorateRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper
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
                Message = _localizer[Constants.MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace }
            );
            _ = await _unitOfWork.SaveAsync();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllGovernorateRequest request, string lang)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? lang == Constants.Languages.Ar
                    ? nameof(Governorate.NameAR)
                    : nameof(Governorate.NameEN)
                : request.SearchBy;

            var governorates = await _unitOfWork.Governorate.GetAllAsync(request);
            var response = _mapper.Map<List<GovernorateDto>>(governorates);
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = true,
                Message = _localizer[Constants.MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<GovernorateDto>>
                {
                    Items = response,
                    Total = response != null ? response.Count : 0
                }
            };
        }
        catch (Exception ex)
        {
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
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
                Message = _localizer[Constants.MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<BaseResponse> CreateAsync(
        CreateGovernorateRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var governorate = _mapper.Map<Governorate>(request);
            governorate.CreateBy = userId;
            governorate = await _unitOfWork.Governorate.AddaAync(governorate);
            var result = _mapper.Map<GovernorateDto>(governorate);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Create,
                NotificationIcons.Add,
                Constants.EntitsKeys.Governorates,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.Fail].ToString(),
                };
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            transaction.Commit();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<BaseResponse> UpdateAsync(
        UpdateGovernorateRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            _mapper.Map(request, governorate);
            governorate.ModifyBy = userId;
            governorate.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<GovernorateDto>(governorate);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Update,
                NotificationIcons.Edit,
                Constants.EntitsKeys.Governorates,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.Fail].ToString(),
                };
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            transaction.Commit();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<BaseResponse> ToggleAvtiveAsync(
        ToggleAvtiveGovernorateRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            governorate.ModifyBy = userId;
            governorate.ModifyAt = DateTime.UtcNow;
            governorate.IsActive = !governorate.IsActive;
            var result = _mapper.Map<GovernorateDto>(governorate);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Toggle,
                NotificationIcons.Edit,
                Constants.EntitsKeys.Governorates,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.Fail].ToString(),
                };
            }
        }
        catch (Exception ex)
        {
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<BaseResponse> DeleteAsync(
        DeleteGovernorateRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
            governorate.DeletedBy = userId;
            governorate.DeletedAt = DateTime.UtcNow;
            governorate.IsDeleted = true;
            var result = _mapper.Map<GovernorateDto>(governorate);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Delete,
                NotificationIcons.Delete,
                Constants.EntitsKeys.Governorates,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = _localizer[Constants.MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.Fail].ToString(),
                };
            }
        }
        catch (Exception ex)
        {
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
            );
            _ = await _unitOfWork.SaveAsync();
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    #region helpers
    private async Task SendNotification(
        OperationTypeEnum operation,
        string icon,
        string entity,
        string userId,
        string userName
    )
    {
        _ = await _unitOfWork.Notification.AddNotificationAsync(
            new Notification
            {
                CreateBy = userId,
                CreateName = userName,
                operationTypeEnum = operation,
                Icon = icon,
                EntityName = entity
            }
        );
    }

    private async Task<bool> IsDone(int modifyRows)
    {
        var count = await _unitOfWork.SaveAsync();
        return count == modifyRows;
    }
    #endregion
}
