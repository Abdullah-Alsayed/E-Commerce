using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Error.Dto;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
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
    private readonly IHttpContextAccessor _httpContext;
    private readonly IStringLocalizer<ErrorService> _localizer;

    private string _userId = Constants.System;
    private string _userName = Constants.System;
    private string _lang = Constants.Languages.Ar;

    public ErrorService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ErrorService> localizer,
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
            _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString() ?? Languages.Ar;
        #endregion
    }

    public async Task<BaseResponse> GetAllAsync(EntitiesEnum Entity)
    {
        try
        {
            var errors =
                Entity != 0
                    ? await _unitOfWork.ErrorLog.GetAllAsync(
                        x => x.Entity == Entity,
                        null,
                        y => y.Date,
                        Constants.OrderBY.Descending
                    )
                    : await _unitOfWork.ErrorLog.GetAllAsync(
                        null,
                        y => y.Date,
                        Constants.OrderBY.Descending
                    );
            var group = errors.GroupBy(x => x.Date.Date);
            var response = group
                .Select(x => new ErrorDto
                {
                    Date = x.Key,
                    Errors = x.ToList(),
                    Count = x.Count()
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
            await _unitOfWork.ErrorLog.ErrorLog(ex, OperationTypeEnum.GetAll, EntitiesEnum.Errors);
            return new BaseResponse
            {
                IsSuccess = false,
                Message = _localizer[MessageKeys.Fail].ToString()
            };
        }
    }
}
