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
    public class PromoCodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromoCodeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindPromoCode))]
        public async Task<IActionResult> FindPromoCode(int ID)
        {
            var PromoCode = await _unitOfWork.PromoCode.FindAsync(ID);
            if (PromoCode == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(PromoCode);
        }

        [HttpGet]
        // [Route(nameof(FindAllPromoCode))]
        public async Task<IActionResult> FindAllPromoCode()
        {
            var PromoCodes = await _unitOfWork.PromoCode.GetAllAsync();
            if (PromoCodes == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(PromoCodes);
        }

        [HttpPost]
        //[Route(nameof(CreatePromoCode))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreatePromoCode(PromoCodeDto dto)
        {
            var Mapping = _mapper.Map<PromoCode>(dto);
            Mapping.CreateAt = DateTime.Now;
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            var PromoCode = await _unitOfWork.PromoCode.AddaAync(Mapping);
            if (PromoCode == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(PromoCode);
        }

        // PUT api/<PromoCodeController>/5
        [HttpPut]
        //[Route(nameof(UpdatePromoCode))]
        public async Task<IActionResult> UpdatePromoCode(int ID, PromoCodeDto dto)
        {
            var PromoCode = await _unitOfWork.PromoCode.FindAsync(ID);
            if (PromoCode == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, PromoCode);
                PromoCode.ModifyAt = DateTime.Now;
                PromoCode.ModifyBy = _unitOfWork.User.GetUserID(User);
                await _unitOfWork.SaveAsync();
                return Ok(PromoCode);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        [Route(nameof(DeletePromoCode))]
        public async Task<IActionResult> DeletePromoCode(int ID)
        {
            var PromoCode = await _unitOfWork.PromoCode.FindAsync(ID);
            if (!_unitOfWork.PromoCode.Delete(PromoCode))
                return NotFound(Constants.Errors.NotFound);
            else
                await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
