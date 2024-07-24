using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.BLL.Features.ContactUses.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [AllowAnonymous]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _service;

        public ContactUsController(IContactUsService service) => _service = service;

        [HttpPost]
        public async Task<BaseResponse> CreateContactUs([FromForm] CreateContactUsRequest request)
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

        [HttpGet]
        public async Task<BaseResponse> GetAllContactUs([FromQuery] GetAllContactUsRequest request)
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
    }
}
