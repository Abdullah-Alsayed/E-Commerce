using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service) => _service = service;

    [Authorize(Policy = Permissions.Role.View)]
    public IActionResult List() => View();

    [HttpPost]
    [Authorize(Policy = Permissions.Role.View)]
    public async Task<IActionResult> Table([FromBody] DataTableRequest request)
    {
        var search = request?.Search?.Value;
        var dir = request?.Order?.FirstOrDefault()?.Dir ?? "desc";
        bool isDescending = (dir == "desc");
        var columns = new List<string>
        {
            nameof(Role.Name),
            nameof(Role.NameEn),
            nameof(Role.Description),
            nameof(Role.RoleType),
            nameof(Role.CreateAt),
        };
        string sortColumn = columns[request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1];
        var response = await _service.GetAllAsync(
            new GetAllRoleRequest
            {
                IsDescending = isDescending,
                SortBy = sortColumn,
                PageSize = request?.Length ?? 0,
                PageIndex = request != null ? (request.Start / request.Length) : 0,
                SearchFor = search,
            }
        );
        var jsonResponse = new
        {
            draw = request?.Draw ?? 0,
            recordsTotal = response?.Count ?? 0,
            recordsFiltered = response?.Count ?? 0,
            data = response?.Result.Items ?? new List<RoleDto>()
        };

        return Json(jsonResponse);
    }

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
    public async Task<BaseResponse> CreateRole([FromForm] CreateRoleRequest request)
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
    public async Task<BaseResponse> UpdateRole([FromForm] UpdateRoleRequest request)
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
    public async Task<BaseResponse> UpdateRoleClaims([FromForm] UpdateRoleClaimsRequest request) // Change FromForm to FromBody
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
    public async Task<BaseResponse> UpdateUserClaims([FromForm] UpdateUserClaimsRequest request)
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

    [HttpGet]
    public async Task<IActionResult> GetClaims(string id)
    {
        try
        {
            var result = await _service.GetClaimsAsync(new BaseRequest { ID = Guid.Parse(id) });
            if (result.IsSuccess)
                return PartialView("_Claims", result.Result);
            else
                return BadRequest(new { IsSuccess = false, Message = result.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { IsSuccess = false, Message = ex.Message });
        }
    }
}
