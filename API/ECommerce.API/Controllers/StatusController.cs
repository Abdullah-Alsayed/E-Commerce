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
    public class StatusController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatusController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindStatus))]
        public async Task<IActionResult> FindStatus(int ID)
        {
            var Status = await _unitOfWork.Status.FindAsync(ID);
            if (Status == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Status);
        }

        [HttpGet]
        // [Route(nameof(FindAllStatus))]
        public async Task<IActionResult> FindAllStatus()
        {
            var Statuss = await _unitOfWork.Status.GetAllAsync();
            if (Statuss == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Statuss);
        }

        [HttpPost]
        //[Route(nameof(CreateStatus))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateStatus(StatusDto dto)
        {
            var Mapping = _mapper.Map<Status>(dto);
            Mapping.CreateAt = DateTime.Now;
            Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

            var Status = await _unitOfWork.Status.AddaAync(Mapping);
            if (Status == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(Status);
        }

        // PUT api/<StatusController>/5
        [HttpPut]
        //[Route(nameof(UpdateStatus))]
        public async Task<IActionResult> UpdateStatus(int ID, StatusDto dto)
        {
            var Status = await _unitOfWork.Status.FindAsync(ID);
            if (Status == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, Status);
                Status.ModifyAt = DateTime.Now;
                Status.ModifyBy = _unitOfWork.User.GetUserID(User);
                await _unitOfWork.SaveAsync();
                return Ok(Status);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteStatus))]
        public async Task<IActionResult> SetAvtiveStatus(int ID)
        {
            var Status = await _unitOfWork.Status.FindAsync(ID);
            if (Status == null)
                return NotFound(Constants.Errors.NotFound);
            else
                Status.IsActive = _unitOfWork.Status.ToggleAvtive(Status.IsActive);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
