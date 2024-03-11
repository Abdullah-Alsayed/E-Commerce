using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Vouchers.Dtos;
using ECommerce.BLL.Features.Vouchers.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Vouchers.Services;

public class VoucherService : IVoucherService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IStringLocalizer<VoucherService> _localizer;

    private string _userId = Constants.System;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public VoucherService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<VoucherService> localizer,
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
            cfg.CreateMap<Voucher, VoucherDto>().ReverseMap();
            cfg.CreateMap<Voucher, CreateVoucherRequest>().ReverseMap();
            cfg.CreateMap<Voucher, UpdateVoucherRequest>().ReverseMap();
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
            await ErrorLog(ex, OperationTypeEnum.Find);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllVoucherRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Voucher.Name)
                : request.SearchBy;

            var vouchers = await _unitOfWork.Voucher.GetAllAsync(request);
            var response = _mapper.Map<List<VoucherDto>>(vouchers);
            return new BaseResponse<BaseGridResponse<List<VoucherDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<VoucherDto>>
                {
                    Items = response,
                    Total = response != null ? response.Count : 0
                }
            };
        }
        catch (Exception ex)
        {
            await ErrorLog(ex, OperationTypeEnum.GetAll);
            return new BaseResponse
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
            voucher.CreateBy = _userId;
            voucher = await _unitOfWork.Voucher.AddaAync(voucher);
            var result = _mapper.Map<VoucherDto>(voucher);
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
            await ErrorLog(ex, OperationTypeEnum.Create);
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
            voucher.ModifyBy = _userId;
            voucher.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<VoucherDto>(voucher);
            #region Send Notification
            await SendNotification(OperationTypeEnum.Update);
            modifyRows++;
            #endregion

            #region Log
            await LogHistory(OperationTypeEnum.Update);
            modifyRows++;
            #endregion

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
            await ErrorLog(ex, OperationTypeEnum.Update);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveVoucherRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var voucher = await _unitOfWork.Voucher.FindAsync(request.ID);
            voucher.ModifyBy = _userId;
            voucher.ModifyAt = DateTime.UtcNow;
            voucher.IsActive = !voucher.IsActive;
            var result = _mapper.Map<VoucherDto>(voucher);
            #region Send Notification
            await SendNotification(OperationTypeEnum.Toggle);
            modifyRows++;
            #endregion

            #region Log
            await LogHistory(OperationTypeEnum.Toggle);
            modifyRows++;
            #endregion

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
            await ErrorLog(ex, OperationTypeEnum.Toggle);
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
            voucher.DeletedBy = _userId;
            voucher.DeletedAt = DateTime.UtcNow;
            voucher.IsDeleted = true;
            var result = _mapper.Map<VoucherDto>(voucher);
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
            await ErrorLog(ex, OperationTypeEnum.Delete);
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
            await ErrorLog(ex, OperationTypeEnum.Search);
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
        await _unitOfWork.History.AddaAync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Voucher
            }
        );
    }

    private async Task ErrorLog(Exception ex, OperationTypeEnum action)
    {
        await _unitOfWork.ErrorLog.ErrorLog(ex, action, EntitiesEnum.Voucher);
        _ = await _unitOfWork.SaveAsync();
    }
    #endregion
}
