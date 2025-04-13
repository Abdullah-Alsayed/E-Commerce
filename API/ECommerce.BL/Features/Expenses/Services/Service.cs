using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Expenses.Dtos;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Expenses.Services;

public class ExpenseService : IExpenseService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IHostEnvironment _environment;
    private readonly IStringLocalizer<ExpenseService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Languages.Ar;

    public ExpenseService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ExpenseService> localizer,
        IUserContext userContext,
        IHostEnvironment environment
    )
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _userContext = userContext;
        _environment = environment;

        #region initilize mapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.CreateMap<Expense, ExpenseDto>().ReverseMap();
            cfg.CreateMap<Expense, CreateExpenseRequest>().ReverseMap();
            cfg.CreateMap<Expense, UpdateExpenseRequest>().ReverseMap();
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> FindAsync(FindExpenseRequest request)
    {
        try
        {
            var Expense = await _unitOfWork.Expense.FindAsync(request.ID);
            var result = _mapper.Map<ExpenseDto>(Expense);
            return new BaseResponse<ExpenseDto>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.View, EntitiesEnum.Expense);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> GetAllAsync(GetAllExpenseRequest request)
    {
        try
        {
            request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                ? nameof(Expense.Reference)
                : request.SearchBy;

            var result = await _unitOfWork.Expense.GetAllAsync(request);
            var response = _mapper.Map<List<ExpenseDto>>(result.list);
            return new BaseResponse<BaseGridResponse<List<ExpenseDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Total = response != null ? result.count : 0,
                Result = new BaseGridResponse<List<ExpenseDto>>
                {
                    Items = response,
                    Total = response != null ? result.count : 0,
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.GetAll, EntitiesEnum.Expense);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> CreateAsync(CreateExpenseRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var expense = _mapper.Map<Expense>(request);
            expense.CreateBy = _userId;
            expense.PhotoPath = await _unitOfWork.Expense.UploadPhotoAsync(
                request.FormFile,
                Constants.PhotoFolder.Expense
            );
            expense = await _unitOfWork.Expense.AddAsync(expense, _userId);
            var result = _mapper.Map<ExpenseDto>(expense);
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
                return new BaseResponse<ExpenseDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Create, EntitiesEnum.Expense);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> UpdateAsync(UpdateExpenseRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Expense = await _unitOfWork.Expense.FindAsync(request.ID);
            _mapper.Map(request, Expense);
            Expense.ModifyBy = _userId;
            Expense.ModifyAt = DateTime.UtcNow;
            Expense.PhotoPath = await _unitOfWork.Expense.UploadPhotoAsync(
                request.FormFile,
                Constants.PhotoFolder.Expense,
                Expense.PhotoPath
            );
            var result = _mapper.Map<ExpenseDto>(Expense);
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
                return new BaseResponse<ExpenseDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Update, EntitiesEnum.Expense);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }

    public async Task<BaseResponse> DeleteAsync(DeleteExpenseRequest request)
    {
        using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
        var modifyRows = 0;
        try
        {
            var Expense = await _unitOfWork.Expense.FindAsync(request.ID);
            Expense.DeletedBy = _userId;
            Expense.DeletedAt = DateTime.UtcNow;
            Expense.IsDeleted = true;
            var result = _mapper.Map<ExpenseDto>(Expense);
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
                return new BaseResponse<ExpenseDto>
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Delete, EntitiesEnum.Expense);
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
            var result = _unitOfWork.Expense.SearchEntity();
            return new BaseResponse<List<string>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = result
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.Search, EntitiesEnum.Expense);
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
                Entity = EntitiesEnum.Expense
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
                Entity = EntitiesEnum.Expense
            },
            _userId
        );
    }

    #endregion
}
