using ECommerce.BLL.Features.Histories.Dtos;
using ECommerce.BLL.Features.Histories.Requests;
using ECommerce.BLL.Features.Histories.Services;
using ECommerce.BLL.Request;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Portal.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryService _service;

        public HistoryController(IHistoryService service) => _service = service;

        [Authorize(Policy = Permissions.Histories.View)]
        public IActionResult List() => View();

        [HttpPost]
        [Authorize(Policy = Permissions.Histories.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);

            var columns = new List<string>
            {
                nameof(HistoryDto.Action),
                nameof(HistoryDto.Entity),
                nameof(HistoryDto.UserName),
                nameof(HistoryDto.CreateAt),
            };

            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllHistoryRequest
                {
                    IsDescending = isDescending,
                    SortBy = sortColumn,
                    PageSize = request?.Length ?? Constants.PageSize,
                    PageIndex = request?.PageIndex ?? Constants.PageIndex,
                    SearchFor = search
                }
            );

            var jsonResponse = new
            {
                draw = request?.Draw ?? 0,
                recordsTotal = response?.Total ?? 0,
                recordsFiltered = response?.Total ?? 0,
                data = response?.Result.Items ?? new List<HistoryDto>()
            };

            return Json(jsonResponse);
        }
    }
}
