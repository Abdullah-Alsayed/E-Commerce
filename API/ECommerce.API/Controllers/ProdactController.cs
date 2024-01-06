using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using AutoMapper;
using System.Collections.Generic;
using ECommerce.DAL.Entity;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> FindProduct(Guid ID)
        {
            var Product = await _unitOfWork.Product.GetItemAsync(x => x.ID == ID, new[] { "User" });
            if (Product == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Product);
        }

        [HttpGet]
        //[Route(nameof(FindAllProduct))]
        public async Task<IActionResult> FindAllProduct()
        {
            var Products = await _unitOfWork.Product.GetAllAsync(
                new[] { "User", "ProductImgs" },
                o => o.Title,
                Constants.OrderBY.Ascending
            );
            if (Products == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Products);
        }

        [HttpPost]
        //[Route(nameof(CreateProduct))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateProduct([FromQuery] ProductDto dto)
        {
            var Entity = _mapper.Map<Product>(dto);
            Entity.CreateAt = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User);

            var Product = await _unitOfWork.Product.AddaAync(Entity);
            if (Product == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
            {
                await _unitOfWork.SaveAsync();
                await _unitOfWork.ProductPhoto.AddPhotos(Product.ID, dto.Files);
                await _unitOfWork.SaveAsync();
                return Ok(Product);
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        //[Route(nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct(int ID, [FromQuery] ProductDto dto)
        {
            var Product = await _unitOfWork.Product.FindAsync(ID);

            if (Product == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Product);
                Product.ModifyAt = DateTime.Now;
                Product.ModifyBy = _unitOfWork.User.GetUserID(User);

                await _unitOfWork.ProductPhoto.AddPhotos(Product.ID, dto.Files);
                await _unitOfWork.SaveAsync();
                return Ok(Product);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(int ID)
        {
            var Product = await _unitOfWork.Product.FindAsync(ID);
            if (Product == null)
                return NotFound(Constants.Errors.NotFound);
            else
                Product.IsDeleted = true;
            await _unitOfWork.SaveAsync();
            return Ok();
        }

        public async Task<IActionResult> AddToChart(int ID)
        {
            var Product = await _unitOfWork.Product.FindAsync(ID);

            return Ok();
        }
    }
}
