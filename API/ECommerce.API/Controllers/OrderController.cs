using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Features.Orders.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service) => _service = service;

        [HttpGet]
        public async Task<BaseResponse> FindOrder([FromQuery] FindOrderRequest request)
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
        public async Task<BaseResponse> GetAllOrder([FromQuery] GetAllOrderRequest request)
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
        public async Task<BaseResponse> CreateOrder([FromBody] CreateOrderRequest request)
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

        [HttpDelete]
        public async Task<BaseResponse> DeleteOrder([FromHeader] DeleteOrderRequest request)
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

        [HttpPut]
        public async Task<BaseResponse> AcceptOrder0([FromBody] AcceptOrderRequest request)
        {
            try
            {
                return await _service.AcceptAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateStatusOrder(
            [FromBody] UpdateStatusOrderRequest request
        )
        {
            try
            {
                return await _service.UpdateStatusAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
