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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindCategory))]
        public async Task<IActionResult> FindCategory(Guid ID)
        {
            var Category = await _unitOfWork.Category.GetItemAsync(
                x => x.ID == ID,
                new[] { "User" }
            );
            if (Category == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Category);
        }

        [HttpGet]
        //[Route(nameof(FindAllCategory))]
        public async Task<IActionResult> FindAllCategory()
        {
            var Categorys = await _unitOfWork.Category.GetAllAsync(
                new[] { "User" },
                o => o.NameAR,
                Constants.OrderBY.Descending
            );
            if (Categorys == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Categorys);
        }

        [HttpPost]
        //[Route(nameof(CreateCategory))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateCategory([FromQuery] CategoryDto dto)
        {
            Category Entity = _mapper.Map<Category>(dto);
            Entity.CreateAt = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User);
            Entity.PhotoPath = await _unitOfWork.Category.UplodPhoto(
                dto.File,
                Constants.PhotoFolder.Categorys
            );

            var Category = await _unitOfWork.Category.AddaAync(Entity);
            if (Category == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Category);
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        //[Route(nameof(UpdateCategory))]
        public async Task<IActionResult> UpdateCategory(int ID, [FromQuery] CategoryDto dto)
        {
            var Category = await _unitOfWork.Category.FindAsync(ID);
            if (Category == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Category);
                Category.ModifyAt = DateTime.Now;
                Category.ModifyBy = _unitOfWork.User.GetUserID(User);
                Category.PhotoPath = await _unitOfWork.Category.UplodPhoto(
                    dto.File,
                    Constants.PhotoFolder.Categorys,
                    Category.PhotoPath
                );
                await _unitOfWork.SaveAsync();
                return Ok(Category);
            }
        }

        //[HttpDelete]
        ////[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteCategory))]
        //public async Task<IActionResult> DeleteCategory(int ID)
        //{
        //    var Category = await _unitOfWork.Category.FindAsync(ID);
        //    if (!_unitOfWork.Category.Delete(Category))
        //        return NotFound(Constants.Errors.NotFound);
        //    else
        //      await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
