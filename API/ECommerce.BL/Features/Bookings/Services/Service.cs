using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Bookings.Dtos;
using ECommerce.BLL.Features.Bookings.Requests;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;
using static ECommerce.Core.PermissionsClaims.Permissions;

namespace ECommerce.BLL.Features.Bookings.Services;

public class BookingService : IBookingService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<BookingService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public BookingService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<BookingService> localizer,
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
            cfg.CreateMap<Booking, BookingDto>().ReverseMap();
            cfg.CreateMap<Booking, CreateBookingRequest>().ReverseMap();
            cfg.CreateMap<Booking, UpdateBookingRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindBookingRequest request)
    {
        try
        {
            var Booking = await _unitOfWork.ProductModule.Booking.FindAsync(request.ID);
            var result = _mapper.Map<BookingDto>(Booking);
            return new BaseResponse<BookingDto>
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
                EntitiesEnum.Booking
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllBookingRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Booking.Id)
                : request.SearchBy;

            var result = await _unitOfWork.ProductModule.Booking.GetAllAsync(request);
            var response = _mapper.Map<List<BookingDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<BookingDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<BookingDto>>
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
                EntitiesEnum.Booking
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateBookingRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Booking = _mapper.Map<Booking>(request);
            Booking = await _unitOfWork.ProductModule.Booking.AddAsync(Booking, _userId);
            var result = _mapper.Map<BookingDto>(Booking);

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
                return new BaseResponse<BookingDto>
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
                EntitiesEnum.Booking
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateBookingRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var booking = await _unitOfWork.ProductModule.Booking.FindAsync(request.ID);
            _mapper.Map(request, booking);
            _unitOfWork.ProductModule.Booking.Update(booking, _userId);
            var result = _mapper.Map<BookingDto>(booking);

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
                return new BaseResponse<BookingDto>
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
                EntitiesEnum.Booking
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> NotifyAsync(NotifyBookingRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var booking = await _unitOfWork.ProductModule.Booking.FindAsync(request.ID);
            booking.IsNotified = true;
            _unitOfWork.ProductModule.Booking.Update(booking, _userId);
            var result = _mapper.Map<BookingDto>(booking);

            //#region Send Notification
            //await SendNotification(OperationTypeEnum.Notify);
            //modifyRows++;
            //#endregion

            //#region Log
            //await LogHistory(OperationTypeEnum.Notify);
            //modifyRows++;
            //#endregion

            modifyRows++;
            if (await _unitOfWork.IsDone(modifyRows))
            {
                await transaction.CommitAsync();
                return new BaseResponse<BookingDto>
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
                EntitiesEnum.Booking
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteBookingRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var booking = await _unitOfWork.ProductModule.Booking.FindAsync(request.ID);
            _unitOfWork.ProductModule.Booking.Delete(booking, _userId);
            var result = _mapper.Map<BookingDto>(booking);

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
                return new BaseResponse<BookingDto>
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
                EntitiesEnum.Booking
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
            var result = _unitOfWork.ProductModule.Booking.SearchEntity();
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
                EntitiesEnum.Booking
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
                Entity = EntitiesEnum.Booking,
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
                Entity = EntitiesEnum.Booking
            },
            _userId
        );
    }

    #endregion
}
