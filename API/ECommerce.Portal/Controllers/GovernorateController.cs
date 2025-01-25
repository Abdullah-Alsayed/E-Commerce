using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Features.Governorates.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Authorize]
public class GovernorateController : Controller
{
    private readonly IGovernorateService _service;

    public GovernorateController(IGovernorateService service) => _service = service;

    [HttpGet]
    public async Task<BaseResponse> FindGovernorate([FromQuery] FindGovernorateRequest request)
    {
        try
        {
            return await _service.FindAsync(request);
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
            return await _service.GetAllAsync(request);
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
            return await _service.GetSearchEntityAsync();
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPost]
    public async Task<BaseResponse> CreateGovernorate([FromForm] CreateGovernorateRequest request)
    {
        try
        {
            return await _service.CreateAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateGovernorate([FromForm] UpdateGovernorateRequest request)
    {
        try
        {
            return await _service.UpdateAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> ToggleAvtiveGovernorate(
        [FromForm] ToggleAvtiveGovernorateRequest request
    )
    {
        try
        {
            return await _service.ToggleAvtiveAsync(request);
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
            return await _service.DeleteAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }
}
