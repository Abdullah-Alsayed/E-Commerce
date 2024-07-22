using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.Features.Stocks.Services;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;

        public StockController(IStockService service) => _service = service;

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
        public async Task<BaseResponse> CreateStock([FromBody] CreateStockRequest request)
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
        public async Task<BaseResponse> ReturnStock([FromBody] ReturnStockRequest request)
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
