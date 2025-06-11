using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Error.Dto;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Errors.Services;

public class ErrorService : IErrorService
{
    IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IStringLocalizer<ErrorService> _localizer;

    private Guid _userId = Guid.Empty;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public ErrorService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ErrorService> localizer,
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
        });
        _mapper = new Mapper(config);
        #endregion initilize mapper

        #region Get User Data From Token
        _userId = _userContext.UserId.Value;

        _userName = _userContext.UserName.Value;

        _lang = _userContext.Language.Value;
        #endregion
    }

    public async Task<BaseResponse> GetAllAsync(EntitiesEnum Entity)
    {
        try
        {
            var errors =
                Entity != 0
                    ? await _unitOfWork.ContentModule.ErrorLog.GetAllAsync(
                        x => x.Entity == Entity,
                        null,
                        y => y.Date,
                        Constants.OrderBY.Descending
                    )
                    : await _unitOfWork.ContentModule.ErrorLog.GetAllAsync(
                        null,
                        y => y.Date,
                        Constants.OrderBY.Descending
                    );
            var group = errors.GroupBy(x => x.Date.Date);
            var response = group
                .Select(x => new ErrorDto
                {
                    Date = x.Key,
                    Errors = x.Select(err => new ErrorLogDto
                        {
                            Date = err.Date,
                            Endpoint = err.Endpoint,
                            Entity = err.Entity.ToString(),
                            ID = err.ID,
                            Message = err.Message,
                            Operation = err.Operation.ToString(),
                            Source = err.Source,
                            StackTrace = err.StackTrace
                        })
                        .ToList(),
                    Count = x.Count(),
                })
                .ToList();
            return new BaseResponse<BaseGridResponse<List<ErrorDto>>>
            {
                IsSuccess = true,
                Message = _localizer[MessageKeys.Success].ToString(),
                Result = new BaseGridResponse<List<ErrorDto>>
                {
                    Items = response,
                    Total = response.Count
                }
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                ex,
                OperationTypeEnum.GetAll,
                EntitiesEnum.Errors
            );
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }
}
