using ECommerce.BLL.DTO;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ECommerce.Services;
using System.Collections.Generic;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User {
                FitsrName = dto.FitsrName,          lastName    = dto.lastName,
                Address   = dto.Address,             Age        = dto.Age,
                CreatDate = DateTime.Now,           language    = Constants.languages.Arabic,
                Email     = dto.Email.ToLower()   , UserName    = dto.UserName.ToLower(),
                Gander    = Constants.Gender.Male , PhoneNumber = dto.PhoneNumber
            };
            if (await _unitOfWork.User.EmailExistesAsync(user.Email))
            {
                if(await _unitOfWork.User.UserNameExistesAsync(user.UserName))
                { 
                    if(_unitOfWork.User.PhoneExistes(user.PhoneNumber))
                    { 
                        var Result = await _unitOfWork.User.RegisterAsync(user, dto.Password);
                        if (Result.Succeeded)
                        {
                            await _unitOfWork.User.LoginAsync(user,true);
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
                return BadRequest(Constants.Errors.Emailexists);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Result = await _unitOfWork.User.LoginAsync(dto.UserName.ToLower(),dto.Password,dto.Rememberme);
            if (Result.Succeeded)
                return Ok(Result);
            else
                return BadRequest(Constants.Errors.LoginFiled);
        }

        [HttpGet]
        [Route("Userinfo")]
        public  IActionResult Userinfo()
        {
            var UserID = _unitOfWork.User.GetUserID(User);
            var UserName = _unitOfWork.User.GetUserName(User);
            var IsAuthenticated = _unitOfWork.User.IsAuthenticated(User);
            var InfoList = new List<string>();
            InfoList.Add(UserName); InfoList.Add(UserID);
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
