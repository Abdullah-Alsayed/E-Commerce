using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Settings.Requests;
using ECommerce.BLL.Features.Settings.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _service;

        public SettingController(ISettingService service) => _service = service;

        [HttpGet]
        public async Task<BaseResponse> GetSetting()
        {
            try
            {
                return await _service.GetAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateSetting([FromBody] UpdateSettingRequest request)
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
