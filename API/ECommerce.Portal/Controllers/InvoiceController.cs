using ECommerce.BLL.Features.Invoices.Dtos;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Features.Invoices.Services;
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
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service) => _service = service;

        [Authorize(Policy = Permissions.Invoice.View)]
        public IActionResult List() => View();

        #region CRUD
        [HttpPost]
        [Authorize(Policy = Permissions.Invoice.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(InvoiceDto.OrderID),
                nameof(InvoiceDto.Order.Total),
                nameof(InvoiceDto.Order.Count),
                nameof(InvoiceDto.Order.Discount),
                nameof(InvoiceDto.Order.Tax),
                nameof(InvoiceDto.Order.IsOffLine),
                nameof(InvoiceDto.IsReturn),
                nameof(InvoiceDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllInvoiceRequest
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
                data = response?.Result.Items ?? new List<InvoiceDto>()
            };

            return Json(jsonResponse);
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllInvoiceRequest request)
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

        public async Task<BaseResponse> Get(Guid id)
        {
            try
            {
                return await _service.FindAsync(new FindInvoiceRequest { ID = id });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
