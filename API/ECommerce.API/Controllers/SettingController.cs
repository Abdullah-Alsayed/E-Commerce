using AutoMapper;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;

        public SettingController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSetting()
        {
            var Setting = await _unitOfWork.Setting.GetItemAsync(s => s.ID != Guid.Empty, null);
            if (Setting == null)
                return NotFound(Constants.Errors.NotFound);
            else
                return Ok(Setting);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSetting(SettingDto Dto)
        {
            var Setting = await _unitOfWork.Setting.GetItemAsync(s => s.ID != Guid.Empty);
            if (Setting == null)
                return NotFound(Constants.Errors.NotFound);
            else
            {
                _Mapper.Map(Dto, Setting);
                Setting.ModifyBy = _unitOfWork.User.GetUserID(User);
                Setting.ModifyAt = DateTime.Now;
                await _unitOfWork.SaveAsync();
                return Ok(Setting);
            }
        }
    }
}
