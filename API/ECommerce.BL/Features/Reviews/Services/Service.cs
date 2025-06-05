using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Reviews.Dtos;
using ECommerce.BLL.Features.Reviews.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;
using static ECommerce.Core.PermissionsClaims.Permissions;

namespace ECommerce.BLL.Features.Reviews.Services;

public class ReviewService : IReviewService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<ReviewService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public ReviewService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ReviewService> localizer,
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
            cfg.CreateMap<ProductReview, ReviewDto>().ReverseMap();
            cfg.CreateMap<ProductReview, CreateReviewRequest>().ReverseMap();
            cfg.CreateMap<ProductReview, UpdateReviewRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.View, EntitiesEnum.Review);
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

            var result = await _unitOfWork.Review.GetAllAsync(request);
            var response = _mapper.Map<List<ReviewDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<ReviewDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<ReviewDto>>
                {
                    Items = response,
                    Total = response != null ? result.count : 0,
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
            Review = await _unitOfWork.Review.AddAsync(Review, _userId);
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
            var review = await _unitOfWork.Review.FindAsync(request.ID);
            _mapper.Map(request, review);
            _unitOfWork.Review.Update(review, _userId);
            var result = _mapper.Map<ReviewDto>(review);
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
            var review = await _unitOfWork.Review.FindAsync(request.ID);
            _unitOfWork.Review.Delete(review, _userId);
            var result = _mapper.Map<ReviewDto>(review);

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
        await _unitOfWork.History.AddAsync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Review
            },
            _userId
        );
    }

    #endregion
}
