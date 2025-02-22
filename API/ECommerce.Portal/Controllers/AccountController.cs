using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core.PermissionsClaims;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _service;
        private readonly IRoleService _roleService;

        public AccountController(IUserService service, IRoleService roleService)
        {
            _service = service;
            _roleService = roleService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse> Register(CreateUserRequest request)
        {
            try
            {
                return await _service.RegisterAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            // Validate the ReturnUrl to prevent open redirect vulnerabilities
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            ViewData["ReturnUrl"] = returnUrl; // Pass the ReturnUrl to the view
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SeedData()
        {
            await _service.SeedData();
            return Json(new object { });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse> Login(LoginRequest request)
        {
            try
            {
                var result = await _service.WebLoginAsync(request, HttpContext);
                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> List()
        {
            var response = await _roleService.GetAllAsync(new GetAllRoleRequest { });
            return View(response.Result.Items);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? "desc";
            bool isDescending = (dir == "desc");
            var columns = new List<string>
            {
                nameof(UserDto.FirstName),
                nameof(UserDto.LastName),
                nameof(UserDto.Address),
                nameof(UserDto.Photo),
                nameof(UserDto.PhoneNumber),
                nameof(DAL.Entity.User.Role.Name),
                nameof(UserDto.LastLogin),
                nameof(UserDto.CreateAt),
            };
            string sortColumn = columns[request?.Order?.FirstOrDefault()?.Column ?? 6];
            var response = await _service.GetAllAsync(
                new GetAllUserRequest
                {
                    IsDescending = isDescending,
                    SortBy = sortColumn,
                    PageSize = request?.Length ?? 0,
                    PageIndex = request != null ? (request.Start / request.Length) : 0,
                    SearchFor = search,
                    StaffOnly = true
                }
            );

            var jsonResponse = new
            {
                draw = request?.Draw ?? 0,
                recordsTotal = response?.Count ?? 0,
                recordsFiltered = response?.Count ?? 0,
                data = response?.Result.Items ?? new List<UserDto>()
            };

            return Json(jsonResponse);
        }

        [HttpPut]
        public async Task<BaseResponse> ChangePassword([FromForm] ChangePasswordUserRequest request)
        {
            try
            {
                return await _service.ChangePasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ChangeUserPassword(
            [FromForm] ChangeUserPasswordRequest request
        )
        {
            try
            {
                return await _service.ChangeUserPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ForgotPassword([FromForm] ForgotPasswordUserRequest request)
        {
            try
            {
                return await _service.ForgotPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ResetPassword([FromForm] ResetPasswordUserRequest request)
        {
            try
            {
                return await _service.ResetPasswordAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ConfirmEmail([FromForm] ConfirmEmailUserRequest request)
        {
            try
            {
                return await _service.ConfirmEmailAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> SendConfirmEmail()
        {
            try
            {
                return await _service.SendConfirmEmailAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPost]
        public async Task<BaseResponse> Create([FromForm] CreateUserRequest request)
        {
            try
            {
                return await _service.CreateAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllUserRequest request)
        {
            try
            {
                return await _service.GetAllAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpDelete]
        public async Task<BaseResponse> Delete(string id)
        {
            try
            {
                return await _service.DeleteAsync(new DeleteUserRequest { ID = Guid.Parse(id) });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> Get(string id)
        {
            try
            {
                return await _service.GetAsync(id);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> UserInfo()
        {
            try
            {
                return await _service.UserInfoAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("Login");
        }

        public async Task<IActionResult> UpdateLanguage(string language, string path)
        {
            var response = await _service.UpdateLanguage(language, HttpContext, Response);
            return Redirect(path);
        }
    }
}
