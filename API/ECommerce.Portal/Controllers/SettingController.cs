using ECommerce.BLL.Features.Settings.Dtos;
using ECommerce.BLL.Features.Settings.Requests;
using ECommerce.BLL.Features.Settings.Services;
using ECommerce.BLL.Response;
using ECommerce.Core.PermissionsClaims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly ISettingService _service;

        public SettingController(ISettingService service) => _service = service;

        [HttpGet]
        [Authorize(Policy = Permissions.Settings.View)]
        public async Task<IActionResult> View()
        {
            try
            {
                var result = await _service.GetAsync();
                return View(result.Result);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Settings.Update)]
        public async Task<BaseResponse> Update([FromForm] UpdateSettingRequest request)
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
    }
}
