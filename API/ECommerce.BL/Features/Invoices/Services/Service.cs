using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Invoices.Dtos;
using ECommerce.BLL.Features.Invoices.Dtos;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Invoices.Services;

public class InvoiceService : IInvoiceService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IStringLocalizer<InvoiceService> _localizer;

    private string _userId = Constants.System;
    private string _userName = Constants.System;
    private string _lang = Languages.Ar;

    public InvoiceService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<InvoiceService> localizer,
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
            cfg.CreateMap<Invoice, InvoiceDto>().ReverseMap();
            cfg.CreateMap<Invoice, CreateInvoiceRequest>().ReverseMap();
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

    public async Task<BaseResponse> FindAsync(FindInvoiceRequest request)
    {
        try
        {
            var Invoice = await _unitOfWork.Invoice.FindAsync(request.ID);
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Find, EntitiesEnum.Invoice);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllInvoiceRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Invoice.ID)
                : request.SearchBy;

            var Invoices = await _unitOfWork.Invoice.GetAllAsync(request);
            var response = _mapper.Map<List<InvoiceDto>>(Invoices);
            return new BaseResponse<BaseGridResponse<List<InvoiceDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<InvoiceDto>>
                {
                    Items = response,
                    Total = response != null ? response.Count : 0
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.GetAll, EntitiesEnum.Invoice);
            return new BaseResponse
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
            Invoice.CreateBy = _userId;
            Invoice = await _unitOfWork.Invoice.AddAsync(Invoice);
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Create, EntitiesEnum.Invoice);
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
            var Invoice = await _unitOfWork.Invoice.FindAsync(request.ID);
            Invoice.DeletedBy = _userId;
            Invoice.DeletedAt = DateTime.UtcNow;
            Invoice.IsDeleted = true;
            var result = _mapper.Map<InvoiceDto>(Invoice);
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Delete, EntitiesEnum.Invoice);
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
            var Invoice = await _unitOfWork.Invoice.GetInvoiceProductsAsync(request);
            Invoice.ModifyBy = _userId;
            Invoice.ModifyAt = DateTime.UtcNow;
            Invoice.IsReturn = true;
            modifyRows += await ReturnProductToStock(Invoice);
            var result = _mapper.Map<InvoiceDto>(Invoice);
            #region Send Notification
            await SendNotification(OperationTypeEnum.Return);
            modifyRows++;
            #endregion

            #region Log
            await LogHistory(OperationTypeEnum.Return);
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Return, EntitiesEnum.Invoice);
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
            var result = _unitOfWork.Invoice.SearchEntity();
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Search, EntitiesEnum.Invoice);
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
        return await _unitOfWork.Stock.ReturnProductToStock(products);
    }

    private async Task<int> RemoverProductFromStock(Invoice invoice)
    {
        var products = GetProductOrders(invoice);
        return await _unitOfWork.Stock.RemoveProductFromStock(products);
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
        _ = await _unitOfWork.Notification.AddNotificationAsync(
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
        await _unitOfWork.History.AddAsync(
            new History
            {
                UserID = _userId,
                Action = action,
                Entity = EntitiesEnum.Invoice
            }
        );
    }

    #endregion
}
