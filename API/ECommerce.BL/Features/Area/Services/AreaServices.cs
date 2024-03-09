using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Area.Dtos;
using ECommerce.BLL.Features.Area.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Area.Services;

public class AreaServices : IAreaServices
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IStringLocalizer<AreaServices> _localizer;

    private string _userId;
    private string _userName;
    private string _lang;

    public AreaServices(
        IUnitOfWork unitOfWork,
        IStringLocalizer<AreaServices> localizer,
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
            cfg.CreateMap<Area, AreaDto>().ReverseMap();
            cfg.CreateMap<Area, CreateAreaRequest>().ReverseMap();
            cfg.CreateMap<Area, UpdateAreaRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _httpContext
            .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntitsKeys.ID)
            ?.Value;

        _userName = _httpContext
            .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntitsKeys.FullName)
            ?.Value;

        _lang =
            _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString() ?? Languages.Ar;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindAreaRequest request)
    {
        try
        {
            var Area = await _unitOfWork.Area.FindAsync(request.ID);
            var result = _mapper.Map<AreaDto>(Area);
            return new BaseResponse<AreaDto>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
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

    public async Task<BaseResponse> GetAllAsync(GetAllAreaRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? _lang == Languages.Ar
                    ? nameof(Area.NameAR)
                    : nameof(Area.NameEN)
                : request.SearchBy;

            var Areas = await _unitOfWork.Area.GetAllAsync(request);
            var response = _mapper.Map<List<AreaDto>>(Areas);
            return new BaseResponse<BaseGridResponse<List<AreaDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
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
                Message = _localizer[MessageKeys.Success].ToString(),
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

    public async Task<BaseResponse> CreateAsync(CreateAreaRequest request)
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var Area = _mapper.Map<Area>(request);
            Area.CreateBy = _userId;
            Area = await _unitOfWork.Area.AddaAync(Area);
            var result = _mapper.Map<AreaDto>(Area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Create,
                NotificationIcons.Add,
                EntitsKeys.Area,
                _userId,
                _userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString(),
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

    public async Task<BaseResponse> UpdateAsync(UpdateAreaRequest request)
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var Area = await _unitOfWork.Area.FindAsync(request.ID);
            _mapper.Map(request, Area);
            Area.ModifyBy = _userId;
            Area.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<AreaDto>(Area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Update,
                NotificationIcons.Edit,
                EntitsKeys.Area,
                _userId,
                _userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString(),
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

    public async Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveAreaRequest request)
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var Area = await _unitOfWork.Area.FindAsync(request.ID);
            Area.ModifyBy = _userId;
            Area.ModifyAt = DateTime.UtcNow;
            Area.IsActive = !Area.IsActive;
            var result = _mapper.Map<AreaDto>(Area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Toggle,
                NotificationIcons.Edit,
                EntitsKeys.Area,
                _userId,
                _userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString(),
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

    public async Task<BaseResponse> DeleteAsync(DeleteAreaRequest request)
    {
        using var transaction = _unitOfWork.Context.Database.BeginTransaction();
        var modifyRows = 0;
        try
        {
            var Area = await _unitOfWork.Area.FindAsync(request.ID);
            Area.DeletedBy = _userId;
            Area.DeletedAt = DateTime.UtcNow;
            Area.IsDeleted = true;
            var result = _mapper.Map<AreaDto>(Area);
            #region Send Notification
            await SendNotification(
                OperationTypeEnum.Delete,
                NotificationIcons.Delete,
                EntitsKeys.Area,
                _userId,
                _userName
            );
            #endregion
            modifyRows = 2;
            if (await IsDone(modifyRows))
            {
                transaction.Commit();
                return new BaseResponse<AreaDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            else
            {
                transaction.Rollback();
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString(),
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
                EntityName = entity,
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
