using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]/[Action]")]
[ApiController]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service) => _service = service;

    [HttpGet]
    public async Task<BaseResponse> FindRole([FromQuery] FindRoleRequest request)
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
    public async Task<BaseResponse> GetAllRole([FromQuery] GetAllRoleRequest request)
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
    public async Task<BaseResponse> CreateRole(CreateRoleRequest request)
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
    public async Task<BaseResponse> UpdateRole(UpdateRoleRequest request)
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

    [HttpDelete]
    public async Task<BaseResponse> DeleteRole(DeleteRoleRequest request)
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

    [HttpPut]
    public async Task<BaseResponse> UpdateRoleClaims(UpdateRoleClaimsRequest request)
    {
        try
        {
            return await _service.UpdateRoleClaimsAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateUserClaims(UpdateUserClaimsRequest request)
    {
        try
        {
            return await _service.UpdateUserClaimsAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateUserRole(UpdateUserRoleRequest request)
    {
        try
        {
            return await _service.UpdateUserRoleAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse { IsSuccess = false, Message = ex.Message };
        }
    }
}
