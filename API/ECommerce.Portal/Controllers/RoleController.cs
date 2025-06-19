using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL.Entity;
using ECommerce.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service) => _service = service;

    #region CRUD
    [Authorize(Policy = Permissions.Roles.View)]
    public IActionResult List() => View();

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.View)]
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
                PageSize = request?.Length ?? Constants.PageSize,
                PageIndex = request?.PageIndex ?? Constants.PageIndex,
                SearchFor = search,
            }
        );
        var jsonResponse = new
        {
            draw = request?.Draw ?? 0,
            recordsTotal = response?.Total ?? 0,
            recordsFiltered = response?.Total ?? 0,
            data = response?.Result.Items ?? new List<RoleDto>()
        };

        return Json(jsonResponse);
    }

    [HttpGet]
    public async Task<IActionResult> FindRole([FromQuery] FindRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.FindAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    public async Task<IActionResult> Get(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.FindAsync(new FindRoleRequest { ID = Guid.Parse(id) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.Create)]
    public async Task<IActionResult> Create([FromForm] CreateRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.CreateAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Roles.Update)]
    public async Task<IActionResult> Update([FromForm] UpdateRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.UpdateAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpDelete]
    [Authorize(Policy = Permissions.Roles.Delete)]
    public async Task<IActionResult> Delete(DeleteRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            var result = await _service.DeleteAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }
    #endregion

    [HttpPut]
    [Authorize(Policy = Permissions.Roles.Permission)]
    public async Task<IActionResult> UpdateRoleClaims([FromForm] UpdateClaimsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.UpdateRoleClaimsAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Roles.Permission)]
    public async Task<IActionResult> UpdateUserClaims([FromForm] UpdateUserClaimsRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.UpdateUserClaimsAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            return Ok(await _service.UpdateUserRoleAsync(request));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Policy = Permissions.Roles.Permission)]
    public async Task<IActionResult> GetRoleClaims(string id)
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

    [Authorize(Policy = Permissions.Roles.Permission)]
    public async Task<IActionResult> GetUserClaims(string id)
    {
        try
        {
            var result = await _service.GetUserClaimsAsync(new BaseRequest { ID = Guid.Parse(id) });
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
