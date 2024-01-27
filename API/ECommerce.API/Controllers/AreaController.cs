using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL.Entity;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
            Area Area = await _unitOfWork.Area.FindAsync(ID);
            return Area == null ? NotFound(Constants.Errors.NotFound) : Ok(Area);
        }

        [HttpGet]
        // [Route(nameof(FindAllArea))]
        public async Task<IActionResult> FindAllArea()
        {
            System.Collections.Generic.IEnumerable<Area> Areas =
                await _unitOfWork.Area.GetAllAsync();
            return Areas == null ? NotFound(Constants.Errors.NotFound) : Ok(Areas);
        }

        [HttpPost]
        //[Route(nameof(CreateArea))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateArea(AreaDto dto)
        {
            Area Mapping = _mapper.Map<Area>(dto);
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            Area Area = await _unitOfWork.Area.AddaAync(Mapping);
            if (Area == null)
            {
                return BadRequest(Constants.Errors.CreateFailed);
            }
            else
            {
                _ = await _unitOfWork.SaveAsync();
            }

            return Ok(Area);
        }

        // PUT api/<AreaController>/5
        [HttpPut]
        //[Route(nameof(UpdateArea))]
        public async Task<IActionResult> UpdateArea(int ID, AreaDto dto)
        {
            Area Area = await _unitOfWork.Area.FindAsync(ID);
            if (Area == null)
            {
                return BadRequest(Constants.Errors.NotFound);
            }
            else
            {
                _ = _mapper.Map(dto, Area);
                Area.ModifyAt = DateTime.Now;
                Area.ModifyBy = _unitOfWork.User.GetUserID(User);
                _ = await _unitOfWork.SaveAsync();
                return Ok(Area);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteArea))]
        public async Task<IActionResult> SetAvtiveArea(int ID)
        {
            Area Area = await _unitOfWork.Area.FindAsync(ID);
            if (Area == null)
            {
                return NotFound(Constants.Errors.NotFound);
            }
            else
            {
                Area.IsActive = _unitOfWork.Area.ToggleAvtive(Area.IsActive);
            }

            _ = await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
