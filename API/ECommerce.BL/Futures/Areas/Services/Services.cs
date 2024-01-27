using AutoMapper;
using ECommerce.BLL.Futures.Areas.Dtos;
using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.BLL.Futures.Areas.Validators;
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

namespace ECommerce.BLL.Futures.Areas.Services;

public class AreaServices : IAreaServices
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<CreateAreaValidator> _localizer;

    public AreaServices(IUnitOfWork unitOfWork, IStringLocalizer<CreateAreaValidator> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;

        #region initilize mapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.CreateMap<Area, AreaDto>().ReverseMap();
            cfg.CreateMap<Area, CreateAreaRequest>().ReverseMap();
            cfg.CreateMap<Area, UpdateAreaRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper
    }

    public async Task<BaseResponse> FindAsync(FindAreaRequest request)
    {
        try
        {
            var area = await _unitOfWork.Area.FindAsync(request.ID);
            var result = _mapper.Map<AreaDto>(area);
            return new BaseResponse<AreaDto>
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

    public async Task<BaseResponse> GetAllAsync(GetAllAreaRequest request, string lang)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? lang == Constants.Languages.Ar
                    ? nameof(Area.NameAR)
                    : nameof(Area.NameEN)
                : request.SearchBy;

            var areas = await _unitOfWork.Area.GetAllAsync(request);
            var response = _mapper.Map<List<AreaDto>>(areas);
            return new BaseResponse<BaseGridResponse<List<AreaDto>>>
            {
                IsSuccess = true,
                Message = _localizer[Constants.MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<AreaDto>>
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
            var result = _unitOfWork.Area.SearchEntity();
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
        CreateAreaRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var area = _mapper.Map<Area>(request);
            area.CreateBy = userId;
            area = await _unitOfWork.Area.AddaAync(area);
            var result = _mapper.Map<AreaDto>(area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Create,
                NotificationIcons.Add,
                Constants.EntitsKeys.Areas,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
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
        UpdateAreaRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.Area.FindAsync(request.ID);
            _mapper.Map(request, area);
            area.ModifyBy = userId;
            area.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<AreaDto>(area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Update,
                NotificationIcons.Edit,
                Constants.EntitsKeys.Areas,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
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
        ToggleAvtiveAreaRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.Area.FindAsync(request.ID);
            area.ModifyBy = userId;
            area.ModifyAt = DateTime.UtcNow;
            area.IsActive = !area.IsActive;
            var result = _mapper.Map<AreaDto>(area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Toggle,
                NotificationIcons.Edit,
                Constants.EntitsKeys.Areas,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
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
        DeleteAreaRequest request,
        string userId,
        string userName
    )
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.Area.FindAsync(request.ID);
            area.DeletedBy = userId;
            area.DeletedAt = DateTime.UtcNow;
            area.IsDeleted = true;
            var result = _mapper.Map<AreaDto>(area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Delete,
                NotificationIcons.Delete,
                Constants.EntitsKeys.Areas,
                userId,
                userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
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
