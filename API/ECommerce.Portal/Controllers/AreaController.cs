using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Areas.Dtos;
using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Areas.Services;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Features.Governorates.Services;
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
    public class AreaController : Controller
    {
        private readonly IAreaService _service;

        private readonly IGovernorateService _governorateService;

        public AreaController(IAreaService service, IGovernorateService governorateService)
        {
            _service = service;
            _governorateService = governorateService;
        }

        [Authorize(Policy = Permissions.Area.View)]
        public async Task<IActionResult> List()
        {
            var response = await _governorateService.GetAllAsync(
                new GetAllGovernorateRequest { IsActive = true }
            );
            return View(response.Result.Items);
        }

        #region CRUD
        [HttpPost]
        [Authorize(Policy = Permissions.Area.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(AreaDto.NameAR),
                nameof(AreaDto.NameEN),
                nameof(AreaDto.GovernorateID),
                nameof(AreaDto.IsActive),
                nameof(AreaDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllAreaRequest
                {
                    IsDescending = isDescending,
                    SortBy = sortColumn,
                    PageSize = request?.Length ?? Constants.PageSize,
                    PageIndex = request?.PageIndex ?? Constants.PageIndex,
                    SearchFor = search,
                    GovernorateID = request.ItemId
                }
            );

            var jsonResponse = new
            {
                draw = request?.Draw ?? 0,
                recordsTotal = response?.Total ?? 0,
                recordsFiltered = response?.Total ?? 0,
                data = response?.Result.Items ?? new List<AreaDto>()
            };

            return Json(jsonResponse);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Area.Create)]
        public async Task<IActionResult> Create([FromForm] CreateAreaRequest request)
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
        [Authorize(Policy = Permissions.Area.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateAreaRequest request)
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
        [Authorize(Policy = Permissions.Area.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.DeleteAsync(
                    new DeleteAreaRequest { ID = Guid.Parse(id) }
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllAreaRequest request)
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
        public async Task<BaseResponse> ToggleActive([FromBody] ToggleActiveAreaRequest request)
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
                return await _service.FindAsync(new FindAreaRequest { ID = id });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
