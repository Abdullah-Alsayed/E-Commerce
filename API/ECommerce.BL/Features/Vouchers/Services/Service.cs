using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Vouchers.Dtos;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Vouchers.Services;

public class VoucherService : IVoucherService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<VoucherService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public VoucherService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<VoucherService> localizer,
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
            cfg.CreateMap<Voucher, VoucherDto>().ReverseMap();
            cfg.CreateMap<Voucher, CreateVoucherRequest>().ReverseMap();
            cfg.CreateMap<Voucher, UpdateVoucherRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindVoucherRequest request)
    {
        try
        {
            var voucher = await _unitOfWork.Voucher.FindAsync(request.ID);
            var result = _mapper.Map<VoucherDto>(voucher);
            return new BaseResponse<VoucherDto>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.View, EntitiesEnum.Voucher);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse<BaseGridResponse<List<VoucherDto>>>> GetAllAsync(
        GetAllVoucherRequest request
    )
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Voucher.Name)
                : request.SearchBy;

            var result = await _unitOfWork.Voucher.GetAllAsync(request);
            var response = _mapper.Map<List<VoucherDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<VoucherDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<VoucherDto>>
                {
                    Items = response,
                    Total = response != null ? result.count : 0,
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.GetAll, EntitiesEnum.Voucher);
            return new BaseResponse<BaseGridResponse<List<VoucherDto>>>
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateVoucherRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var voucher = _mapper.Map<Voucher>(request);
            voucher = await _unitOfWork.Voucher.AddAsync(voucher, _userId);
            var result = _mapper.Map<VoucherDto>(voucher);

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
                return new BaseResponse<VoucherDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Create, EntitiesEnum.Voucher);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateVoucherRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var voucher = await _unitOfWork.Voucher.FindAsync(request.ID);
            _mapper.Map(request, voucher);
            _unitOfWork.Voucher.Update(voucher, _userId);
            var result = _mapper.Map<VoucherDto>(voucher);

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
                return new BaseResponse<VoucherDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Update, EntitiesEnum.Voucher);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveVoucherRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var voucher = await _unitOfWork.Voucher.FindAsync(request.ID);
            _unitOfWork.Voucher.ToggleActive(voucher, _userId);
            var result = _mapper.Map<VoucherDto>(voucher);

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
                return new BaseResponse<VoucherDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Toggle, EntitiesEnum.Voucher);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteVoucherRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var voucher = await _unitOfWork.Voucher.FindAsync(request.ID);
            _unitOfWork.Voucher.Delete(voucher, _userId);
            var result = _mapper.Map<VoucherDto>(voucher);

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
                return new BaseResponse<VoucherDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Delete, EntitiesEnum.Voucher);
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
            var result = _unitOfWork.Voucher.SearchEntity();
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Search, EntitiesEnum.Voucher);
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
        _ = await _unitOfWork.Notification.AddNotificationAsync(
            new Notification
            {
                CreateBy = _userId,
                CreateName = _userName,
                OperationType = action,
                Entity = EntitiesEnum.Voucher
            }
        );
    }

    private async Task LogHistory(OperationTypeEnum action)
    {
        await _unitOfWork.History.AddAsync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Voucher
            },
            _userId
        );
    }

    #endregion
}
