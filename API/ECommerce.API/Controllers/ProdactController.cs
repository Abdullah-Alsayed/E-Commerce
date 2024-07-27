using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Products.Services;
using ECommerce.BLL.Features.Products.Validators;
using ECommerce.BLL.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service) => _service = service;

        [HttpGet]
        public async Task<BaseResponse> FindProduct([FromQuery] FindProductRequest request)
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
        public async Task<BaseResponse> GetProductItems([FromQuery] GetProductItemsRequest request)
        {
            try
            {
                return await _service.GetProductItems(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAllProduct([FromQuery] GetAllProductRequest request)
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
        [Consumes("multipart/form-data")]
        public async Task<BaseResponse> CreateProduct([FromForm] CreateProductRequest request)
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
        public async Task<BaseResponse> UpdateProduct([FromForm] UpdateProductRequest request)
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

        [HttpDelete]
        public async Task<BaseResponse> DeleteProduct( DeleteProductRequest request)
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
