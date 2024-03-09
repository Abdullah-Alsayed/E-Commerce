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
    public class ContactUsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactUsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route(nameof(FindContactUs))]
        public async Task<IActionResult> FindContactUs(int ID)
        {
            var ContactUs = await _unitOfWork.ContactUs.FindAsync(ID);
            if (ContactUs == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(ContactUs);
        }

        [HttpGet]
        // [Route(nameof(FindAllContactUs))]
        public async Task<IActionResult> FindAllContactUs()
        {
            var ContactUss = await _unitOfWork.ContactUs.GetAllAsync();
            if (ContactUss == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(ContactUss);
        }

        [HttpPost]
        //[Route(nameof(CreateContactUs))]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> CreateContactUs(ContactUsDto dto)
        {
            var Mapping = _mapper.Map<ContactUs>(dto);

            var ContactUs = await _unitOfWork.ContactUs.AddaAync(Mapping);
            if (ContactUs == null)
                return BadRequest(Constants.Errors.CreateFailed);
            else
                await _unitOfWork.SaveAsync();
            return Ok(ContactUs);
        }

        // PUT api/<ContactUsController>/5
        [HttpPut]
        //[Route(nameof(UpdateContactUs))]
        public async Task<IActionResult> UpdateContactUs(int ID, ContactUsDto dto)
        {
            var ContactUs = await _unitOfWork.ContactUs.FindAsync(ID);
            if (ContactUs == null)
                return BadRequest(Constants.Errors.NotFound);
            else
            {
                _mapper.Map(dto, ContactUs);
                await _unitOfWork.SaveAsync();
                return Ok(ContactUs);
            }
        }

        [HttpDelete]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        //[Route(nameof(DeleteContactUs))]
        public async Task<IActionResult> DeletContactUs(int ID)
        {
            var ContactUs = await _unitOfWork.ContactUs.FindAsync(ID);
            if (ContactUs == null)
                return NotFound(Constants.Errors.NotFound);
            else
                _unitOfWork.ContactUs.Delete(ContactUs);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
