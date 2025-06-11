using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Feedbacks.Dtos;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Feedbacks.Services
{
    public class FeedbackService : IFeedbackService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<FeedbackService> _localizer;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public FeedbackService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<FeedbackService> localizer,
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
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Feedback, FeedbackDto>().ReverseMap();
                cfg.CreateMap<Feedback, CreateFeedbackRequest>().ReverseMap();
                cfg.CreateMap<Feedback, UpdateFeedbackRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindFeedbackRequest request)
        {
            try
            {
                var Feedback = await _unitOfWork.ContentModule.Feedback.FindAsync(request.ID);
                var result = _mapper.Map<FeedbackDto>(Feedback);
                return new BaseResponse<FeedbackDto>
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
                    EntitiesEnum.Feedback
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<FeedbackDto>>>> GetAllAsync(
            GetAllFeedbackRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(Feedback.Comment)
                    : request.SearchBy;

                var result = await _unitOfWork.ContentModule.Feedback.GetAllAsync(
                    request,
                    new List<string> { nameof(User) }
                );
                var response = _mapper.Map<List<FeedbackDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<FeedbackDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,
                    Result = new BaseGridResponse<List<FeedbackDto>>
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
                    EntitiesEnum.Feedback
                );
                return new BaseResponse<BaseGridResponse<List<FeedbackDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateFeedbackRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var feedback = _mapper.Map<Feedback>(request);
                var user = await _unitOfWork.UserModule.User.FindUserByNameAsync(Constants.System);
                _userId = user?.Id ?? Guid.Empty;
                feedback = await _unitOfWork.ContentModule.Feedback.AddAsync(feedback, _userId);
                var result = _mapper.Map<FeedbackDto>(feedback);

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
                    return new BaseResponse<FeedbackDto>
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
                    EntitiesEnum.Feedback
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateFeedbackRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var feedback = await _unitOfWork.ContentModule.Feedback.FindAsync(request.ID);
                _mapper.Map(request, feedback);
                _unitOfWork.ContentModule.Feedback.Update(feedback, _userId);
                var result = _mapper.Map<FeedbackDto>(feedback);
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
                    return new BaseResponse<FeedbackDto>
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
                    EntitiesEnum.Feedback
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteFeedbackRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var feedback = await _unitOfWork.ContentModule.Feedback.FindAsync(request.ID);
                _unitOfWork.ContentModule.Feedback.Delete(feedback, _userId);
                var result = _mapper.Map<FeedbackDto>(feedback);

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
                    return new BaseResponse<FeedbackDto>
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
                    EntitiesEnum.Feedback
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
                var result = _unitOfWork.ContentModule.Feedback.SearchEntity();
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
                    EntitiesEnum.Feedback
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
            _ = await _unitOfWork.ContentModule.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Entity = EntitiesEnum.Feedback
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Feedback
                },
                _userId
            );

        #endregion
    }
}
