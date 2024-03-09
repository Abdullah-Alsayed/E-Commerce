using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Colors.Dtos;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Colors.Services
{
    public class ColorService : IColorService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer<ColorService> _localizer;

        private string _userId = Constants.System;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public ColorService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ColorService> localizer,
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
                cfg.CreateMap<Color, ColorDto>().ReverseMap();
                cfg.CreateMap<Color, CreateColorRequest>().ReverseMap();
                cfg.CreateMap<Color, UpdateColorRequest>().ReverseMap();
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
                _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
                ?? Languages.Ar;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindColorRequest request)
        {
            try
            {
                var color = await _unitOfWork.Color.FindAsync(request.ID);
                var result = _mapper.Map<BrandDto>(color);
                return new BaseResponse<BrandDto>
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

        public async Task<BaseResponse> GetAllAsync(GetAllColorRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(Governorate.NameAR)
                        : nameof(Governorate.NameEN)
                    : request.SearchBy;

                var colors = await _unitOfWork.Color.GetAllAsync(request);
                var response = _mapper.Map<List<BrandDto>>(colors);
                return new BaseResponse<BaseGridResponse<List<BrandDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<BrandDto>>
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

        public async Task<BaseResponse> CreateAsync(CreateColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = _mapper.Map<Color>(request);
                color.CreateBy = _userId;
                color = await _unitOfWork.Color.AddaAync(color);
                var result = _mapper.Map<BrandDto>(color);
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
                    return new BaseResponse<BrandDto>
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
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = await _unitOfWork.Color.FindAsync(request.ID);
                _mapper.Map(request, color);
                color.ModifyBy = _userId;
                color.ModifyAt = DateTime.UtcNow;
                var result = _mapper.Map<BrandDto>(color);
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
                    return new BaseResponse<BrandDto>
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
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = await _unitOfWork.Color.FindAsync(request.ID);
                color.DeletedBy = _userId;
                color.DeletedAt = DateTime.UtcNow;
                color.IsDeleted = true;
                var result = _mapper.Map<BrandDto>(color);
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
                    return new BaseResponse<BrandDto>
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
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.Color.SearchEntity();
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
                    new ErrorLog { Source = ex.Source, Message = ex.Message, }
                );
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
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
                    Icon = action.ToString(),
                    Entity = EntitiesEnum.Color
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
                    Entity = EntitiesEnum.Color
                }
            );
        }

        private async Task ErrorLog(Exception ex, OperationTypeEnum action)
        {
            _unitOfWork.Context.ChangeTracker.Clear();
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    Operation = action,
                    Entity = EntitiesEnum.Color
                }
            );
        }

        #endregion
    }
}
