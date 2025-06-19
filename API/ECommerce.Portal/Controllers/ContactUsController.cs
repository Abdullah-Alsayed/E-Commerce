using ECommerce.BLL.Features.ContactUses.Dtos;
using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.BLL.Features.ContactUses.Services;
using ECommerce.BLL.Features.ContactUss.Requests;
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
    public class ContactUsController : Controller
    {
        private readonly IContactUsService _service;

        public ContactUsController(IContactUsService service) => _service = service;

        [Authorize(Policy = Permissions.ContactUses.View)]
        public IActionResult List() => View();

        #region CRUD
        [HttpPost]
        [Authorize(Policy = Permissions.ContactUses.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(ContactUsDto.Name),
                nameof(ContactUsDto.Phone),
                nameof(ContactUsDto.Subject),
                nameof(ContactUsDto.Message),
                nameof(ContactUsDto.IsActive),
                nameof(ContactUsDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllContactUsRequest
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
                data = response?.Result.Items ?? new List<ContactUsDto>()
            };

            return Json(jsonResponse);
        }

        [HttpDelete]
        [Authorize(Policy = Permissions.ContactUses.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.DeleteAsync(
                    new DeleteContactUsRequest { ID = Guid.Parse(id) }
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllContactUsRequest request)
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
                return await _service.FindAsync(new FindContactUsRequest { ID = id });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
