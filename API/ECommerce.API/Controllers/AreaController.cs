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
    public class AreaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindArea))]
        public async Task<IActionResult> FindArea(int ID)
        {
            var Area = await _unitOfWork.Area.FindAsync(ID);
            if (Area == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Area);
        }

        [HttpGet]
        // [Route(nameof(FindAllArea))]
        public async Task<IActionResult> FindAllArea()
        {
            var Areas = await _unitOfWork.Area.GetAllAsync();
            if (Areas == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Areas);
        }

        [HttpPost]
        //[Route(nameof(CreateArea))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateArea(AreaDto dto)
        {
            var Mapping = _mapper.Map<Area>(dto);
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            var Area = await _unitOfWork.Area.AddaAync(Mapping);
            if (Area == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Area);
        }

        // PUT api/<AreaController>/5
        [HttpPut]
        //[Route(nameof(UpdateArea))]
        public async Task<IActionResult> UpdateArea(int ID, AreaDto dto)
        {
            var Area = await _unitOfWork.Area.FindAsync(ID);
            if (Area == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Area);
                Area.ModifyAt = DateTime.Now;
                Area.ModifyBy = _unitOfWork.User.GetUserID(User);
                await _unitOfWork.SaveAsync();
                return Ok(Area);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteArea))]
        public async Task<IActionResult> SetAvtiveArea(int ID)
        {
            var Area = await _unitOfWork.Area.FindAsync(ID);
            if (Area == null)
                return NotFound(Constants.Errors.NotFound);
            else
                Area.IsActive = _unitOfWork.Area.SetAvtive(Area.IsActive);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
