using ECommerce.BLL.Features.Statuses.Dtos;
using ECommerce.BLL.Features.Statuses.Requests;
using ECommerce.BLL.Features.Statuses.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        private readonly IStatusService _service;

        public StatusController(IStatusService service) => _service = service;

        [Authorize(Policy = Permissions.Statuses.View)]
        public IActionResult List() => View();

        #region CRUD
        [HttpPost]
        [Authorize(Policy = Permissions.Statuses.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(StatusDto.NameAR),
                nameof(StatusDto.NameEN),
                nameof(StatusDto.Order),
                nameof(StatusDto.IsActive),
                nameof(StatusDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllStatusRequest
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
                data = response?.Result.Items ?? new List<StatusDto>()
            };

            return Json(jsonResponse);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Statuses.Create)]
        public async Task<IActionResult> Create([FromForm] CreateStatusRequest request)
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
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Statuses.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateStatusRequest request)
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
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = Permissions.Statuses.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.DeleteAsync(
                    new DeleteStatusRequest { ID = Guid.Parse(id) }
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllStatusRequest request)
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

        [HttpPut]
        public async Task<BaseResponse> ToggleActive([FromBody] ToggleActiveStatusRequest request)
        {
            try
            {
                return await _service.ToggleActiveAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> Get(Guid id)
        {
            try
            {
                return await _service.FindAsync(new FindStatusRequest { ID = id });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
