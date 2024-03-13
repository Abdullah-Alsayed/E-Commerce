using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Reviews.Dtos;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Reviews.Services;

public class ReviewService : IReviewService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IStringLocalizer<ReviewService> _localizer;

    private string _userId = Constants.System;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public ReviewService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ReviewService> localizer,
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
            cfg.CreateMap<ProductReview, ReviewDto>().ReverseMap();
            cfg.CreateMap<ProductReview, CreateReviewRequest>().ReverseMap();
            cfg.CreateMap<ProductReview, UpdateReviewRequest>().ReverseMap();
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

    public async Task<BaseResponse> FindAsync(FindReviewRequest request)
    {
        try
        {
            var Review = await _unitOfWork.Review.FindAsync(request.ID);
            var result = _mapper.Map<ReviewDto>(Review);
            return new BaseResponse<ReviewDto>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Find, EntitiesEnum.Review);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllReviewRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(ProductReview.Review)
                : request.SearchBy;

            var Reviews = await _unitOfWork.Review.GetAllAsync(request);
            var response = _mapper.Map<List<ReviewDto>>(Reviews);
            return new BaseResponse<BaseGridResponse<List<ReviewDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<ReviewDto>>
                {
                    Items = response,
                    Total = response != null ? response.Count : 0
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.GetAll, EntitiesEnum.Review);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateReviewRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Review = _mapper.Map<ProductReview>(request);
            Review.CreateBy = _userId;
            Review = await _unitOfWork.Review.AddaAync(Review);
            var result = _mapper.Map<ReviewDto>(Review);
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
                return new BaseResponse<ReviewDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Create, EntitiesEnum.Review);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateReviewRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Review = await _unitOfWork.Review.FindAsync(request.ID);
            _mapper.Map(request, Review);
            Review.ModifyBy = _userId;
            Review.ModifyAt = DateTime.UtcNow;
            var result = _mapper.Map<ReviewDto>(Review);
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
                return new BaseResponse<ReviewDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Update, EntitiesEnum.Review);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteReviewRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Review = await _unitOfWork.Review.FindAsync(request.ID);
            Review.DeletedBy = _userId;
            Review.DeletedAt = DateTime.UtcNow;
            Review.IsDeleted = true;
            var result = _mapper.Map<ReviewDto>(Review);
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
                return new BaseResponse<ReviewDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Delete, EntitiesEnum.Review);
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
            var result = _unitOfWork.Review.SearchEntity();
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Search, EntitiesEnum.Review);
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
                Entity = EntitiesEnum.Review,
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
                Entity = EntitiesEnum.Review
            }
        );
    }

    #endregion
}
