using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Areas.Dtos;
using ECommerce.BLL.Features.Governorates.Dtos;
using ECommerce.BLL.Features.Orders.Dtos;
using ECommerce.BLL.Features.Orders.Dtos;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Features.Products.Dtos;
using ECommerce.BLL.Features.Statuses.Dtos;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Orders.Services
{
    public class OrderService : IOrderService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer<OrderService> _localizer;

        private string _userId = Constants.System;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public OrderService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<OrderService> localizer,
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
                cfg.CreateMap<Order, OrderDto>().ReverseMap();
                cfg.CreateMap<Area, AreaDto>().ReverseMap();
                cfg.CreateMap<Status, StatusDto>().ReverseMap();
                cfg.CreateMap<Governorate, GovernorateDto>().ReverseMap();
                cfg.CreateMap<DAL.Entity.ProductOrder, ProductOrderDto>().ReverseMap();
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Order, CreateOrderRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.ID)
                ?.Value;

            _userName = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.FullName)
                ?.Value;

            _lang =
                _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
                ?? Languages.Ar;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindOrderRequest request)
        {
            try
            {
                var Order = await _unitOfWork.Order.FindAsync(request.ID);
                var result = _mapper.Map<OrderDto>(Order);
                return new BaseResponse<OrderDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.View, EntitiesEnum.Order);
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetAllAsync(GetAllOrderRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(Order.Address)
                    : request.SearchBy;

                var Orders = await _unitOfWork.Order.GetAllAsync(request);
                var response = _mapper.Map<List<OrderDto>>(Orders);
                return new BaseResponse<BaseGridResponse<List<OrderDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<OrderDto>>
                    {
                        Items = response,
                        Total = response != null ? response.Count : 0
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.GetAll,
                    EntitiesEnum.Order
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateOrderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var order = _mapper.Map<Order>(request);
                order.CreateBy = _userId;
                modifyRows += await _unitOfWork.Order.AddAsync(order, request.Products);
                var result = _mapper.Map<OrderDto>(order);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Create);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Create);
                modifyRows++;
                #endregion


                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<OrderDto>
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
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Create,
                    EntitiesEnum.Order
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteOrderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Order = await _unitOfWork.Order.FindAsync(request.ID);
                Order.DeletedBy = _userId;
                Order.DeletedAt = DateTime.UtcNow;
                Order.IsDeleted = true;
                var result = _mapper.Map<OrderDto>(Order);
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
                    return new BaseResponse<OrderDto>
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
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Delete,
                    EntitiesEnum.Order
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> AcceptAsync(AcceptOrderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Order = await _unitOfWork.Order.FindAsync(request.ID);
                Order.ModifyBy = _userId;
                Order.ModifyAt = DateTime.UtcNow;
                Order.IsAccept = true;
                var result = _mapper.Map<OrderDto>(Order);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Accept);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Accept);
                modifyRows++;
                #endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<OrderDto>
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
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Accept,
                    EntitiesEnum.Order
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateStatusAsync(UpdateStatusOrderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Order = await _unitOfWork.Order.FindAsync(request.ID);
                Order.ModifyBy = _userId;
                Order.ModifyAt = DateTime.UtcNow;
                Order.StatusID = request.StatusID;
                var result = _mapper.Map<OrderDto>(Order);
                #region Send Notification
                await SendNotification(OperationTypeEnum.UpdateStatus);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.UpdateStatus);
                modifyRows++;
                #endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<OrderDto>
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
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.UpdateStatus,
                    EntitiesEnum.Order
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
                var result = _unitOfWork.Order.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Search,
                    EntitiesEnum.Order
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
            _ = await _unitOfWork.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Entity = EntitiesEnum.Order
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Order
                }
            );

        #endregion
    }
}
