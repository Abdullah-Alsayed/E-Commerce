using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> HomePage()
        {
            var Return = new HomeDto()
            {
                categories = await _unitOfWork.Category.GetAllAsync(),
                Setting = await _unitOfWork.Setting.GetItemAsync(s => s.ID != Guid.Empty),
                SliderPhotos = await _unitOfWork.SliderPhoto.GetAllAsync(),
                SubCategories = await _unitOfWork.SubCategory.GetAllAsync(new[] { "Category" }),
                Products = await _unitOfWork.Product.GetAllAsync(
                    new[] { "Category", "Brand", "Unit", "Color", "SubCategory" }
                ),
                ProductPhotos = await _unitOfWork.ProductPhoto.GetAllAsync(),
                Reviews = await _unitOfWork.Review.GetAllAsync(),
            };
            return Ok();
        }
    }
}
