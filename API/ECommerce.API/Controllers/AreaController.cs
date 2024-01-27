using AutoMapper;
using ECommerce.BLL.Futures.Areas.Dtos;
using ECommerce.BLL.Futures.Areas.Requests;
using ECommerce.BLL.Futures.Areas.Services;
using ECommerce.BLL.Futures.Areas.Validators;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static ECommerce.Helpers.Constants;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
[Authorize]
public class AreaController : ControllerBase
{
    private string _userId;
    private string _userName;
    private string _lang;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IAreaServices _services;

    public AreaController(IHttpContextAccessor httpContextAccessor, IAreaServices services)
    {
        _httpContext = httpContextAccessor;
        _services = services;
        #region Get User Data From Token
        _userId = _httpContext.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == Constants.EntitsKeys.ID)
            ?.Value;

        _userName = _httpContext.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == Constants.EntitsKeys.FullName)
            ?.Value;

        _lang =
            _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
            ?? Constants.Languages.Ar;
        #endregion
    }

    [HttpGet]
    public async Task<BaseResponse> FindArea([FromQuery] FindAreaRequest request)
    {
        try
        {
            return await _services.FindAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpGet]
    public async Task<BaseResponse> GetAllArea([FromQuery] GetAllAreaRequest request)
    {
        try
        {
            return await _services.GetAllAsync(request, _lang);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpGet]
    public async Task<BaseResponse> GetSearchEntity()
    {
        try
        {
            return await _services.GetSearchEntityAsync();
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPost]
    public async Task<BaseResponse> CreateArea(CreateAreaRequest request)
    {
        try
        {
            return await _services.CreateAsync(request, _userId, _userName);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateArea(UpdateAreaRequest request)
    {
        try
        {
            return await _services.UpdateAsync(request, _userId, _userName);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> ToggleAvtiveArea(ToggleAvtiveAreaRequest request)
    {
        try
        {
            return await _services.ToggleAvtiveAsync(request, _userId, _userName);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpDelete]
    public async Task<BaseResponse> DeleteArea(DeleteAreaRequest request)
    {
        try
        {
            return await _services.DeleteAsync(request, _userId, _userName);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }
}
