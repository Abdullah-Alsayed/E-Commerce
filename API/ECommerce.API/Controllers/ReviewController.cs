using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        //[Route(nameof(FindReview))]
        public async Task<IActionResult> FindReview(int ID)
        {
            var Review = await _unitOfWork.Review.FindAsync(ID);
            if(Review == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Review);
        }
        [HttpGet]
       // [Route(nameof(FindAllReview))]
        public async Task<IActionResult> FindAllReview()
        {
            var Reviews = await _unitOfWork.Review.GetAllAsync(new[] { "Prodact" } );
            if (Reviews == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Reviews);
        }
        [HttpPost]
        //[Route(nameof(CreateReview))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult>CreateReview([FromForm]ReviewDto dto)
        {
            var Mapping = _mapper.Map<Review>(dto);
            Mapping.CrateDate = DateTime.Now;
            Mapping.UserID = _unitOfWork.User.GetUserID(User);

            var Review = await _unitOfWork.Review.AddaAync(Mapping);
            if (Review == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
               return Ok(Review);
        }

        // PUT api/<ReviewController>/5
        [HttpPut]
        //[Route(nameof(UpdateReview))]
        public async Task<IActionResult> UpdateReview(int ID, [FromForm] ReviewDto dto)
        {
            var Review = await _unitOfWork.Review.FindAsync(ID);
            if (Review ==null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Review);
                await _unitOfWork.SaveAsync();
                return Ok(Review);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteReview))]
        public async Task<IActionResult> SetAvtiveReview(int ID)
        {
            var Review = await _unitOfWork.Review.FindAsync(ID);
            if (Review == null)
                return NotFound(Constants.Errors.NotFound);
            else
                _unitOfWork.Review.Delete(Review);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
