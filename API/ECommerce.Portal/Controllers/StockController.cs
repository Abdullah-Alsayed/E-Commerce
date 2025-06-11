using ECommerce.BLL.Features.Areas.Requests;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.Features.Stocks.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly IStockService _service;

        public StockController(IStockService service) => _service = service;

        [Authorize(Policy = Permissions.Stock.View)]
        public IActionResult List() => View();

        [HttpPost]
        [Authorize(Policy = Permissions.Stock.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(StockDto.Product.Title),
                nameof(StockDto.Vendor.Name),
                nameof(StockDto.Quantity),
                nameof(StockDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllStockRequest
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
                data = response?.Result.Items ?? new List<StockDto>()
            };

            return Json(jsonResponse);
        }

        [HttpGet]
        public async Task<BaseResponse> FindStock([FromQuery] FindStockRequest request)
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
        public async Task<BaseResponse> GetAllStock([FromQuery] GetAllStockRequest request)
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
        public async Task<BaseResponse> CreateStock([FromForm] CreateStockRequest request)
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
        public async Task<BaseResponse> ReturnStock([FromForm] ReturnStockRequest request)
        {
            try
            {
                return await _service.ReturnAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
