using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL.Entity;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User
            {
                FirstName = dto.FitsrName,
                LastName = dto.lastName,
                Address = dto.Address,
                Age = dto.Age,
                Language = Constants.Languages.Arabic,
                Email = dto.Email.ToLower(),
                UserName = dto.UserName.ToLower(),
                Gander = DAL.Enums.UserGanderEnum.Male,
                PhoneNumber = dto.PhoneNumber
            };
            if (await _unitOfWork.User.EmailExistesAsync(user.Email))
            {
                if (await _unitOfWork.User.UserNameExistesAsync(user.UserName))
                {
                    if (_unitOfWork.User.PhoneExistes(user.PhoneNumber))
                    {
                        var Result = await _unitOfWork.User.RegisterAsync(user, dto.Password);
                        if (Result.Succeeded)
                        {
                            await _unitOfWork.User.LoginAsync(user, true);
                            return Ok(user);
                        }
                        else
                        {
                            foreach (var item in Result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return BadRequest(Constants.Errors.PhoneNumbeExists);
                    }
                }
                else
                {
                    return BadRequest(Constants.Errors.UserNameExists);
                }
            }
            else
            {
                return BadRequest(Constants.Errors.Emailexists);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Microsoft.AspNetCore.Identity.SignInResult Result = await _unitOfWork.User.LoginAsync(
                dto.UserName.ToLower(),
                dto.Password,
                dto.Rememberme
            );
            return Result.Succeeded ? Ok(Result) : BadRequest(Constants.Errors.LoginFiled);
        }

        [HttpGet]
        [Route("Userinfo")]
        public IActionResult Userinfo()
        {
            string UserID = _unitOfWork.User.GetUserID(User);
            string UserName = _unitOfWork.User.GetUserName(User);
            _ = _unitOfWork.User.IsAuthenticated(User);
            List<string> InfoList = new() { UserName, UserID };
            return Ok(InfoList);
        }

        [HttpGet]
        [Route("Logof")]
        public async Task<IActionResult> Logof()
        {
            await _unitOfWork.User.LogOffAsync();
            return Ok();
        }
    }
}
