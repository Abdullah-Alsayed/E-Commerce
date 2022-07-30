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
    public class ColorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        //[Route(nameof(FindColor))]
        public async Task<IActionResult> FindColor(int ID)
        {
            var Color = await _unitOfWork.Color.FindAsync(ID);
            if(Color == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Color);
        }
        [HttpGet]
       // [Route(nameof(FindAllColor))]
        public async Task<IActionResult> FindAllColor()
        {
            var Colors = await _unitOfWork.Color.GetAllAsync();
            if (Colors == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Colors);
        }
        [HttpPost]
        //[Route(nameof(CreateColor))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult>CreateColor(ColorDto dto)
        {
                var Mapping = _mapper.Map<Color>(dto);

                var color = await _unitOfWork.Color.AddaAync(Mapping);
                if (color == null)
                    return BadRequest(Constants.Errors.CreateFailed);
                else
                    await _unitOfWork.Notification.AddNotificationAsync(new Notification
                    {
                        UserID = _unitOfWork.User.GetUserID(User),
                        Icon = Constants.NotificationIcons.Add,
                        Title = "AddColor",
                        Subject = "AddColor",
                        Messege = "Addcolor",
                    });
                await _unitOfWork.SaveAsync();
                return Ok(color);
        }

        // PUT api/<ColorController>/5
        [HttpPut]
        //[Route(nameof(UpdateColor))]
        public async Task<IActionResult> UpdateColor(int ID, ColorDto dto)
        {
            var color = await _unitOfWork.Color.FindAsync(ID);
            if (color ==null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, color);
               await _unitOfWork.SaveAsync();
                return Ok(color);
            }
        }

        //[HttpDelete]
        ////[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteColor))]
        //public async Task<IActionResult> DeleteColor(int ID)
        //{
        //    var color = await _unitOfWork.Color.FindAsync(ID);
        //    if (!_unitOfWork.Color.Delete(color))
        //        return NotFound(Constants.Errors.NotFound);
        //    else
        //      await _unitOfWork.SaveAsync();
        //    return Ok();
        //}
    }
}
