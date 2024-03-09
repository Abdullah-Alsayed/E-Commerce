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
    public class UnitController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindUnit))]
        public async Task<IActionResult> FindUnit(int ID)
        {
            var Unit = await _unitOfWork.Unit.FindAsync(ID);
            if (Unit == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Unit);
        }

        [HttpGet]
        //[Route(nameof(FindAllUnit))]
        public async Task<IActionResult> FindAllUnit()
        {
            var Units = await _unitOfWork.Unit.GetAllAsync(
                new[] { "User" },
                o => o.NameAR,
                Constants.OrderBY.Descending
            );
            if (Units == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Units);
        }

        [HttpPost]
        //[Route(nameof(CreateUnit))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateUnit(UnitDto dto)
        {
            Unit Entity = _mapper.Map<Unit>(dto);
            Entity.CreateAt = DateTime.Now;
            Entity.CreateBy = _unitOfWork.User.GetUserID(User);

            var Unit = await _unitOfWork.Unit.AddaAync(Entity);
            if (Unit == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Unit);
        }

        // PUT api/<UnitController>/5
        [HttpPut]
        //[Route(nameof(UpdateUnit))]
        public async Task<IActionResult> UpdateUnit(int ID, UnitDto dto)
        {
            var Unit = await _unitOfWork.Unit.FindAsync(ID);
            if (Unit == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Unit);
                Unit.ModifyBy = _unitOfWork.User.GetUserID(User);
                Unit.ModifyAt = DateTime.Now;
                await _unitOfWork.SaveAsync();
                return Ok(Unit);
            }
        }

        //[HttpDelete]
        ////[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteUnit))]
        //public async Task<IActionResult> DeleteUnit(int ID)
        //{
        //    var Unit = await _unitOfWork.Unit.FindAsync(ID);
        //    if (!_unitOfWork.Unit.Delete(Unit))
        //        return NotFound(Constants.Errors.NotFound);
        //    else
        //      await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
