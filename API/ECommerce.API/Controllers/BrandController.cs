using System;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindBrand))]
        public async Task<IActionResult> FindBrand(Guid ID)
        {
            var Brand = await _unitOfWork.Brand.GetItemAsync(x => x.ID == ID, new[] { "User" });
            if (Brand == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Brand);
        }

        [HttpGet]
        //[Route(nameof(FindAllBrand))]
        public async Task<IActionResult> FindAllBrand()
        {
            var Brands = await _unitOfWork.Brand.GetAllAsync(
                new[] { "User" },
                o => o.NameAR,
                Constants.OrderBY.Descending
            );
            if (Brands == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Brands);
        }

        [HttpPost]
        //[Route(nameof(CreateBrand))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateBrand([FromQuery] BrandDto dto)
        {
            Brand Entity = _mapper.Map<Brand>(dto);
            Entity.CreateAt = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User);
            Entity.PhotoPath = await _unitOfWork.Brand.UplodPhoto(
                dto.File,
                Constants.PhotoFolder.Brands
            );

            var Brand = await _unitOfWork.Brand.AddaAync(Entity);
            if (Brand == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Brand);
        }

        // PUT api/<BrandController>/5
        [HttpPut]
        //[Route(nameof(UpdateBrand))]
        public async Task<IActionResult> UpdateBrand(int ID, [FromQuery] BrandDto dto)
        {
            var Brand = await _unitOfWork.Brand.FindAsync(ID);
            if (Brand == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Brand);
                Brand.ModifyAt = DateTime.Now;
                Brand.ModifyBy = _unitOfWork.User.GetUserID(User);
                Brand.PhotoPath = await _unitOfWork.Brand.UplodPhoto(
                    dto.File,
                    Constants.PhotoFolder.Brands,
                    Brand.PhotoPath
                );
                await _unitOfWork.SaveAsync();
                return Ok(Brand);
            }
        }

        //[HttpDelete]
        ////[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteBrand))]
        //public async Task<IActionResult> DeleteBrand(int ID)
        //{
        //    var Brand = await _unitOfWork.Brand.FindAsync(ID);
        //    if (!_unitOfWork.Brand.Delete(Brand))
        //        return NotFound(Constants.Errors.NotFound);
        //    else
        //      await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
