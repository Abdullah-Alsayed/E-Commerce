using AutoMapper;
using ECommerce.BLL.Futures.Governorates.Dtos;
using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.BLL.Futures.Governorates.Services;
using ECommerce.BLL.Futures.Governorates.Validators;
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
public class GovernorateController : ControllerBase
{
    private string _userId;
    private string _userName;
    private string _lang;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IGovernorateServices _services;

    public GovernorateController(
        IHttpContextAccessor httpContextAccessor,
        IGovernorateServices services
    )
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
    public async Task<BaseResponse> FindGovernorate([FromQuery] FindGovernorateRequest request)
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
    public async Task<BaseResponse> GetAllGovernorate([FromQuery] GetAllGovernorateRequest request)
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
    public async Task<BaseResponse> CreateGovernorate(CreateGovernorateRequest request)
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
    public async Task<BaseResponse> UpdateGovernorate(UpdateGovernorateRequest request)
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
    public async Task<BaseResponse> ToggleAvtiveGovernorate(ToggleAvtiveGovernorateRequest request)
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
    public async Task<BaseResponse> DeleteGovernorate(DeleteGovernorateRequest request)
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
