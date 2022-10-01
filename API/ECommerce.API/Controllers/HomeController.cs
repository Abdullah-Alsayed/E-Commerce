
using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
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

        public async Task<IActionResult> HomePage()
        {
            var Return = new HomeDto()
            {
                categories = await _unitOfWork.Category.GetAllAsync(),
                Setting = await _unitOfWork.Setting.GetItemAsync(s => s.ID > 0),
                Sliders = await _unitOfWork.Slider.GetAllAsync(),
                SubCategories = await _unitOfWork.SubCategory.GetAllAsync(new[] { "Category" }),
                Prodacts = await _unitOfWork.Prodact.GetAllAsync(new[] { "Category", "Brand", "Unit", "Color", "SubCategory" }),
                ProdactImgs = await _unitOfWork.ProdactImg.GetAllAsync(),
                Reviews = await _unitOfWork.Review.GetAllAsync(),
            };
            return Ok();
        }
    }
}
