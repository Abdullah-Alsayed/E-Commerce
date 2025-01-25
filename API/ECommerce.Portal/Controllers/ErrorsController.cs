using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Errors.Services;
using ECommerce.BLL.Response;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private readonly IErrorService _service;

        public ErrorController(IErrorService service) => _service = service;

        [HttpGet]
        public async Task<BaseResponse> GetAllError(EntitiesEnum Entity)
        {
            try
            {
                return await _service.GetAllAsync(Entity);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
