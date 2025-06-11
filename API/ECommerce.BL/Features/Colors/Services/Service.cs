using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Carts.Dtos;
using ECommerce.BLL.Features.Carts.Requests;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.Colors.Dtos;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.Feedbacks.Dtos;
using ECommerce.BLL.Features.Feedbacks.Requests;
using ECommerce.BLL.Features.Orders.Dtos;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Features.Sizes.Dtos;
using ECommerce.BLL.Features.Sizes.Requests;
using ECommerce.BLL.Features.Vendors.Dtos;
using ECommerce.BLL.Features.Vendors.Requests;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
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
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<ColorService> _localizer;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public ColorService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ColorService> localizer,
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
                cfg.CreateMap<Color, ColorDto>().ReverseMap();
                cfg.CreateMap<Color, CreateColorRequest>().ReverseMap();
                cfg.CreateMap<Color, UpdateColorRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindColorRequest request)
        {
            try
            {
                var color = await _unitOfWork.ProductModule.Color.FindAsync(request.ID);
                var result = _mapper.Map<ColorDto>(color);
                return new BaseResponse<ColorDto>
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
                    EntitiesEnum.Color
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<ColorDto>>>> GetAllAsync(
            GetAllColorRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(Color.NameAR)
                        : nameof(Color.NameEN)
                    : request.SearchBy;

                var result = await _unitOfWork.ProductModule.Color.GetAllAsync(request);
                var response = _mapper.Map<List<ColorDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<ColorDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,
                    Result = new BaseGridResponse<List<ColorDto>>
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
                    EntitiesEnum.Color
                );
                return new BaseResponse<BaseGridResponse<List<ColorDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = _mapper.Map<Color>(request);
                color = await _unitOfWork.ProductModule.Color.AddAsync(color, _userId);
                var result = _mapper.Map<ColorDto>(color);

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
                    return new BaseResponse<ColorDto>
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
                    EntitiesEnum.Color
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = await _unitOfWork.ProductModule.Color.FindAsync(request.ID);
                _mapper.Map(request, color);
                _unitOfWork.ProductModule.Color.Update(color, _userId);
                var result = _mapper.Map<ColorDto>(color);

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
                    return new BaseResponse<ColorDto>
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
                    EntitiesEnum.Color
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = await _unitOfWork.ProductModule.Color.FindAsync(request.ID);
                _unitOfWork.ProductModule.Color.Delete(color, _userId);
                var result = _mapper.Map<ColorDto>(color);

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
                    return new BaseResponse<ColorDto>
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
                    EntitiesEnum.Color
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveColorRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var color = await _unitOfWork.ProductModule.Color.FindAsync(request.ID);
                _unitOfWork.ProductModule.Color.ToggleActive(color, _userId);

                var result = _mapper.Map<ColorDto>(color);

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
                    return new BaseResponse<ColorDto>
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
                    EntitiesEnum.Category
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
                var result = _unitOfWork.ProductModule.Color.SearchEntity();
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
                    EntitiesEnum.Color
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
                    Entity = EntitiesEnum.Color
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Color
                },
                _userId
            );

        #endregion
    }
}
