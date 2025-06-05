using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.Features.Sliders.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _service;

        public SliderController(ISliderService service) => _service = service;

        [HttpGet]
        public async Task<BaseResponse> FindSlider([FromQuery] FindSliderRequest request)
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
        public async Task<BaseResponse> GetAllSlider(GetAllSliderRequest request)
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

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<BaseResponse> CreateSlider([FromForm] CreateSliderRequest request)
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
        [Consumes("multipart/form-data")]
        public async Task<BaseResponse> UpdateSlider([FromForm] UpdateSliderRequest request)
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

        [HttpDelete]
        public async Task<BaseResponse> DeleteSlider(DeleteSliderRequest request)
        {
            try
            {
                return await _service.DeleteAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
