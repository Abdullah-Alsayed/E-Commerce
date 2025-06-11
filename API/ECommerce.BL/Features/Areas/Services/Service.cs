using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Areas.Dtos;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.Governorates.Dtos;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Areas.Services;

public class AreaService : IAreaService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<AreaService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public AreaService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<AreaService> localizer,
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
            cfg.CreateMap<Area, AreaDto>().ReverseMap();
            cfg.CreateMap<Area, CreateAreaRequest>().ReverseMap();
            cfg.CreateMap<Area, UpdateAreaRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindAreaRequest request)
    {
        try
        {
            var area = await _unitOfWork.LocationModule.Area.FindAsync(request.ID);
            var result = _mapper.Map<AreaDto>(area);
            return new BaseResponse<AreaDto>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.View,
                EntitiesEnum.Area
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse<BaseGridResponse<List<AreaDto>>>> GetAllAsync(
        GetAllAreaRequest request
    )
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? _lang == Languages.Ar
                    ? nameof(Area.NameAR)
                    : nameof(Area.NameEN)
                : request.SearchBy;

            var result =
                request.GovernorateID.Value != Guid.Empty
                    ? await _unitOfWork.LocationModule.Area.GetAllAsync(
                        request,
                        x => x.GovernorateID == request.GovernorateID.Value,
                        new List<string> { nameof(Governorate) }
                    )
                    : await _unitOfWork.LocationModule.Area.GetAllAsync(
                        request,
                        new List<string> { nameof(Governorate) }
                    );

            var response = _mapper.Map<List<AreaDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<AreaDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<AreaDto>>
                {
                    Items = response,
                    Total = response != null ? result.count : 0,
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.GetAll,
                EntitiesEnum.Area
            );
            return new BaseResponse<BaseGridResponse<List<AreaDto>>>
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateAreaRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var area = _mapper.Map<Area>(request);
            area = await _unitOfWork.LocationModule.Area.AddAsync(area, _userId);
            var result = _mapper.Map<AreaDto>(area);

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
                return new BaseResponse<AreaDto>
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
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.Create,
                EntitiesEnum.Area
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateAreaRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.LocationModule.Area.FindAsync(request.ID);
            _mapper.Map(request, area);
            _unitOfWork.LocationModule.Area.Update(area, _userId);
            var result = _mapper.Map<AreaDto>(area);

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
                return new BaseResponse<AreaDto>
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
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.Update,
                EntitiesEnum.Area
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveAreaRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.LocationModule.Area.FindAsync(request.ID);
            _unitOfWork.LocationModule.Area.ToggleActive(area, _userId);
            var result = _mapper.Map<AreaDto>(area);

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
                return new BaseResponse<AreaDto>
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
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.Toggle,
                EntitiesEnum.Area
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteAreaRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var area = await _unitOfWork.LocationModule.Area.FindAsync(request.ID);
            _unitOfWork.LocationModule.Area.Delete(area, _userId);
            var result = _mapper.Map<AreaDto>(area);

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
                return new BaseResponse<AreaDto>
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
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.Delete,
                EntitiesEnum.Area
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
            var result = _unitOfWork.LocationModule.Area.SearchEntity();
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.Search,
                EntitiesEnum.Area
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    #region helpers
    private async Task SendNotification(OperationTypeEnum action)
    {
        _ = await _unitOfWork.ContentModule.Notification.AddNotificationAsync(
            new Notification
            {
                CreateBy = _userId,
                CreateName = _userName,
                OperationType = action,
                Entity = EntitiesEnum.Area,
            }
        );
    }

    private async Task LogHistory(OperationTypeEnum action)
    {
        await _unitOfWork.ContentModule.History.AddAsync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Area
            },
            _userId
        );
    }

    #endregion
}
