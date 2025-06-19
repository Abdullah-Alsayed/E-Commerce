using ECommerce.BLL.Features.Governorates.Dtos;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Features.Governorates.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Authorize]
public class GovernorateController : Controller
{
    private readonly IGovernorateService _service;

    public GovernorateController(IGovernorateService service) => _service = service;

    [Authorize(Policy = Permissions.Governorates.View)]
    public IActionResult List() => View();

    #region CRUD
    [HttpPost]
    [Authorize(Policy = Permissions.Governorates.View)]
    public async Task<IActionResult> Table([FromBody] DataTableRequest request)
    {
        var search = request?.Search?.Value;
        var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
        bool isDescending = (dir == Constants.Descending);
        var columns = new List<string>
        {
            nameof(GovernorateDto.NameAR),
            nameof(GovernorateDto.NameEN),
            nameof(GovernorateDto.Tax),
            nameof(GovernorateDto.IsActive),
            nameof(GovernorateDto.CreateAt),
        };
        string sortColumn = columns[request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1];

        var response = await _service.GetAllAsync(
            new GetAllGovernorateRequest
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
            data = response?.Result.Items ?? new List<GovernorateDto>()
        };

        return Json(jsonResponse);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Governorates.Create)]
    public async Task<IActionResult> Create([FromForm] CreateGovernorateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                }
            );
        }
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Governorates.Update)]
    public async Task<IActionResult> Update([FromForm] UpdateGovernorateRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            var result = await _service.UpdateAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                }
            );
        }
    }

    [HttpDelete]
    [Authorize(Policy = Permissions.Governorates.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

        try
        {
            var result = await _service.DeleteAsync(
                new DeleteGovernorateRequest { ID = Guid.Parse(id) }
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                }
            );
        }
    }

    [HttpGet]
    public async Task<BaseResponse> GetAll([FromQuery] GetAllGovernorateRequest request)
    {
        try
        {
            return await _service.GetAllAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    [HttpPut]
    public async Task<BaseResponse> ToggleActive([FromBody] ToggleActiveGovernorateRequest request)
    {
        try
        {
            return await _service.ToggleActiveAsync(request);
        }
        catch (Exception ex)
        {
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<BaseResponse> Get(Guid id)
    {
        try
        {
            return await _service.FindAsync(new FindGovernorateRequest { ID = id });
        }
        catch (Exception ex)
        {
            return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    #endregion
}
