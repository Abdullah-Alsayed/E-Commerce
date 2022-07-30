using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubCategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route(nameof(FindSubCategory))]
        public async Task<IActionResult> FindSubCategory(int ID)
        {
            var SubCategory = await _unitOfWork.SubCategory.GetItemAsync(x => x.ID == ID, new[] { "User" });
            if (SubCategory == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(SubCategory);
        }
        [HttpGet]
        [Route(nameof(FindAllSubCategory))]
        public async Task<IActionResult> FindAllSubCategory()
        {
            var SubCategorys = await _unitOfWork.SubCategory.GetAllAsync(new[] { "User" }, o => o.NameAR, Constants.OrderBY.Descending);
            if (SubCategorys == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(SubCategorys);
        }
        [HttpPost]
        [Route(nameof(CreateSubCategory))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateSubCategory([FromForm]SubCategoryDto dto)
        {
            SubCategory Entity = _mapper.Map<SubCategory>(dto);
            Entity.CreateDate = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User);
            Entity.Img = await _unitOfWork.SubCategory.UplodImge(dto.File, Constants.ImgFolder.SubCategorys);

            var SubCategory = await _unitOfWork.SubCategory.AddaAync(Entity);
            if (SubCategory == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(SubCategory);
        }

        // PUT api/<SubCategoryController>/5
        [HttpPut]
        [Route(nameof(UpdateSubCategory))]
        public async Task<IActionResult> UpdateSubCategory(int ID, [FromForm] SubCategoryDto dto)
        {
            var SubCategory = await _unitOfWork.SubCategory.FindAsync(ID);
            if (SubCategory == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, SubCategory);
                SubCategory.ModifyAt = DateTime.Now;
                SubCategory.ModifyBy = _unitOfWork.User.GetUserID(User);
                SubCategory.Img = await _unitOfWork.SubCategory.UplodImge(dto.File, Constants.ImgFolder.SubCategorys, SubCategory.Img);
                await _unitOfWork.SaveAsync();
                return Ok(SubCategory);
            }
        }

        //[HttpDelete]
        ////[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteSubCategory))]
        //public async Task<IActionResult> DeleteSubCategory(int ID)
        //{
        //    var SubCategory = await _unitOfWork.SubCategory.FindAsync(ID);
        //    if (!_unitOfWork.SubCategory.Delete(SubCategory))
        //        return NotFound(Constants.Errors.NotFound);
        //    else
        //      await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
