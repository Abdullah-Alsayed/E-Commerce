using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ECommerce.DAL.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GovernorateController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindGovernorate))]
        public async Task<IActionResult> FindGovernorate(int ID)
        {
            var Governorate = await _unitOfWork.Governorate.FindAsync(ID);
            if (Governorate == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Governorate);
        }

        [HttpGet]
        // [Route(nameof(FindAllGovernorate))]
        public async Task<IActionResult> FindAllGovernorate()
        {
            var Governorates = await _unitOfWork.Governorate.GetAllAsync();
            if (Governorates == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Governorates);
        }

        [HttpPost]
        //[Route(nameof(CreateGovernorate))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateGovernorate(GovernorateDto dto)
        {
            var Mapping = _mapper.Map<Governorate>(dto);
            Mapping.CreateAt = DateTime.Now;
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            var Governorate = await _unitOfWork.Governorate.AddaAync(Mapping);
            if (Governorate == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Governorate);
        }

        // PUT api/<GovernorateController>/5
        [HttpPut]
        //[Route(nameof(UpdateGovernorate))]
        public async Task<IActionResult> UpdateGovernorate(int ID, GovernorateDto dto)
        {
            var Governorate = await _unitOfWork.Governorate.FindAsync(ID);
            if (Governorate == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Governorate);
                Governorate.ModifyAt = DateTime.Now;
                Governorate.ModifyBy = _unitOfWork.User.GetUserID(User);
                await _unitOfWork.SaveAsync();
                return Ok(Governorate);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteGovernorate))]
        public async Task<IActionResult> SetAvtiveGovernorate(int ID)
        {
            var Governorate = await _unitOfWork.Governorate.FindAsync(ID);
            if (Governorate == null)
                return NotFound(Constants.Errors.NotFound);
            else
                Governorate.IsActive = _unitOfWork.Governorate.SetAvtive(Governorate.IsActive);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
