using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Constants = ECommerce.Core.Constants;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ColorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        private string _userId = string.Empty;

        public ColorController(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContext,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContext;
            _userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        //[Route(nameof(FindColor))]
        public async Task<IActionResult> FindColor(int ID)
        {
            Color Color = await _unitOfWork.Color.FindAsync(ID);
            return Color == null ? NotFound(Constants.Errors.NotFound) : Ok(Color);
        }

        [HttpGet]
        // [Route(nameof(FindAllColor))]
        public async Task<IActionResult> FindAllColor()
        {
            System.Collections.Generic.IEnumerable<Color> Colors =
                await _unitOfWork.Color.GetAllAsync();
            return Colors == null ? NotFound(Constants.Errors.NotFound) : Ok(Colors);
        }

        [HttpPost]
        //[Route(nameof(CreateColor))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateColor(ColorDto dto)
        {
            try
            {
                Color mapping = _mapper.Map<Color>(dto);
                mapping.CreateBy = _userId;
                Color color = await _unitOfWork.Color.AddaAync(mapping);
                if (color == null)
                {
                    return BadRequest(Constants.Errors.CreateFailed);
                }
                else
                {
                    _ = await _unitOfWork.Notification.AddNotificationAsync(
                        new Notification
                        {
                            CreateBy = _userId,
                            operationTypeEnum = OperationTypeEnum.Create,
                            Icon = Constants.NotificationIcons.Add,
                        }
                    );
                }

                return Ok(color);
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                return BadRequest(ex.Message);
            }
            finally
            {
                _ = await _unitOfWork.SaveAsync();
            }
        }

        // PUT api/<ColorController>/5
        [HttpPut]
        //[Route(nameof(UpdateColor))]
        public async Task<IActionResult> UpdateColor(int ID, ColorDto dto)
        {
            Color color = await _unitOfWork.Color.FindAsync(ID);
            if (color == null)
            {
                return BadRequest(Constants.Errors.NotFound);
            }
            else
            {
                _ = _mapper.Map(dto, color);
                _ = await _unitOfWork.SaveAsync();
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
