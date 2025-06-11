using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Invoices.Dtos;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Features.Orders.Dtos;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Invoices.Services;

public class InvoiceService : IInvoiceService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<InvoiceService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Languages.Ar;

    public InvoiceService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<InvoiceService> localizer,
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
            cfg.CreateMap<Invoice, InvoiceDto>().ReverseMap();
            cfg.CreateMap<Order, OrderDto>().ReverseMap();
            cfg.CreateMap<Invoice, CreateInvoiceRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindInvoiceRequest request)
    {
        try
        {
            var Invoice = await _unitOfWork.OrderModule.Invoice.FindAsync(request.ID);
            var result = _mapper.Map<InvoiceDto>(Invoice);
            return new BaseResponse<InvoiceDto>
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
                EntitiesEnum.Invoice
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse<BaseGridResponse<List<InvoiceDto>>>> GetAllAsync(
        GetAllInvoiceRequest request
    )
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Invoice.Id)
                : request.SearchBy;

            var result = await _unitOfWork.OrderModule.Invoice.GetAllAsync(
                request,
                new List<string> { nameof(Order) }
            );
            var response = _mapper.Map<List<InvoiceDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<InvoiceDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<InvoiceDto>>
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
                EntitiesEnum.Invoice
            );
            return new BaseResponse<BaseGridResponse<List<InvoiceDto>>>
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateInvoiceRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Invoice = _mapper.Map<Invoice>(request);
            Invoice = await _unitOfWork.OrderModule.Invoice.AddAsync(Invoice, _userId);
            modifyRows += await RemoverProductFromStock(Invoice);
            var result = _mapper.Map<InvoiceDto>(Invoice);
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
                return new BaseResponse<InvoiceDto>
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
                EntitiesEnum.Invoice
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteInvoiceRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var invoice = await _unitOfWork.OrderModule.Invoice.FindAsync(request.ID);
            _unitOfWork.OrderModule.Invoice.Delete(invoice, _userId);
            var result = _mapper.Map<InvoiceDto>(invoice);

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
                return new BaseResponse<InvoiceDto>
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
                EntitiesEnum.Invoice
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> ReturnAsync(ReturnInvoiceRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Invoice = await _unitOfWork.OrderModule.Invoice.GetInvoiceProductsAsync(request);
            Invoice.IsReturn = true;
            _unitOfWork.OrderModule.Invoice.Update(Invoice, _userId);
            modifyRows += await ReturnProductToStock(Invoice);
            var result = _mapper.Map<InvoiceDto>(Invoice);

            modifyRows++;
            if (await _unitOfWork.IsDone(modifyRows))
            {
                await transaction.CommitAsync();
                return new BaseResponse<InvoiceDto>
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
                OperationTypeEnum.Return,
                EntitiesEnum.Invoice
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
            var result = _unitOfWork.OrderModule.Invoice.SearchEntity();
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
                EntitiesEnum.Invoice
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    #region helpers
    private async Task<int> ReturnProductToStock(Invoice invoice)
    {
        var products = GetProductOrders(invoice);
        return await _unitOfWork.StockModule.Stock.ReturnProductToStock(products);
    }

    private async Task<int> RemoverProductFromStock(Invoice invoice)
    {
        var products = GetProductOrders(invoice);
        return await _unitOfWork.StockModule.Stock.RemoveProductFromStock(products);
    }

    private List<ProductsOrderRequest> GetProductOrders(Invoice invoice)
    {
        return invoice
            .Order.ProductOrders.Select(product => new ProductsOrderRequest
            {
                ProductID = product.ProductID,
                Quantity = product.Quantity
            })
            .ToList();
    }

    private async Task SendNotification(OperationTypeEnum action)
    {
        _ = await _unitOfWork.ContentModule.Notification.AddNotificationAsync(
            new Notification
            {
                CreateBy = _userId,
                CreateName = _userName,
                OperationType = action,
                Entity = EntitiesEnum.Invoice,
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
                Entity = EntitiesEnum.Invoice
            },
            _userId
        );
    }

    #endregion
}
