// Ignore Spelling: BLL Accessor

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Products.Dtos;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.Features.Vendors.Dtos;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Stocks.Services
{
    public class StockService : IStockService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<StockService> _localizer;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public StockService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<StockService> localizer,
            IUserContext userContext
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userContext = userContext;

            #region initialize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Stock, StockDto>().ReverseMap();
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
                cfg.CreateMap<Vendor, VendorDto>().ReverseMap();
                cfg.CreateMap<Stock, CreateStockRequest>().ReverseMap();
                cfg.CreateMap<Stock, ReturnStockRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindStockRequest request)
        {
            try
            {
                var Stock = await _unitOfWork.StockModule.Stock.FirstAsync(
                    x => x.Id == request.ID,
                    [nameof(Product)]
                );
                var result = _mapper.Map<StockDto>(Stock);
                return new BaseResponse<StockDto>
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
                    EntitiesEnum.Stock
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<StockDto>>>> GetAllAsync(
            GetAllStockRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(Stock.ProductID)
                    : request.SearchBy;

                var result = await _unitOfWork.StockModule.Stock.GetAllAsync(request);
                var response = _mapper.Map<List<StockDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<StockDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,
                    Result = new BaseGridResponse<List<StockDto>>
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
                    EntitiesEnum.Stock
                );
                return new BaseResponse<BaseGridResponse<List<StockDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateStockRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Stock = _mapper.Map<Stock>(request);
                Stock.CreateBy = _userId;
                modifyRows += await _unitOfWork.StockModule.Stock.AddStockAsync(Stock, request);

                var result = _mapper.Map<StockDto>(Stock);

                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<StockDto>
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
                    EntitiesEnum.Stock
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ReturnAsync(ReturnStockRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var stock = await _unitOfWork.StockModule.Stock.FindAsync(request.ID);
                _mapper.Map(request, stock);
                _unitOfWork.StockModule.Stock.Update(stock, _userId);
                var result = _mapper.Map<StockDto>(stock);
                modifyRows = await _unitOfWork.StockModule.Stock.ReturnItemAsync(request);

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
                    return new BaseResponse<StockDto>
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
                    EntitiesEnum.Stock
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
                var result = _unitOfWork.StockModule.Stock.SearchEntity();
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
                    EntitiesEnum.Stock
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
                    Entity = EntitiesEnum.Stock
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Stock
                },
                _userId
            );

        #endregion
    }
}
