using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.Features.Roles.Services;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.Portal.Helpers;
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

        #region CRUD
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
                nameof(UserDto.Address),
                nameof(UserDto.Gander),
                nameof(DAL.Entity.User.Role.Name),
                nameof(UserDto.LastLogin),
                nameof(UserDto.IsActive),
                nameof(UserDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];
            var response = await _service.GetAllAsync(
                new GetAllUserRequest
                {
                    IsDescending = isDescending,
                    SortBy = sortColumn,
                    PageSize = request?.Length ?? Constants.PageSize,
                    PageIndex = request?.PageIndex ?? Constants.PageIndex,
                    SearchFor = search,
                    StaffOnly = true,
                    RoleId = request?.ItemId ?? Guid.Empty
                }
            );

            var jsonResponse = new
            {
                draw = request?.Draw ?? 0,
                recordsTotal = response?.Total ?? 0,
                recordsFiltered = response?.Total ?? 0,
                data = response?.Result.Items ?? new List<UserDto>()
            };

            return Json(jsonResponse);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.CreateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Users.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.UpdateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = Permissions.Users.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.DeleteAsync(
                    new DeleteUserRequest { ID = Guid.Parse(id) }
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
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

        [HttpPut]
        public async Task<BaseResponse> ToggleActive([FromBody] BaseRequest request)
        {
            try
            {
                return await _service.ToggleActive(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> Get(Guid id)
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

        public async Task<IActionResult> UpdateLanguage(string language, string path)
        {
            var response = await _service.UpdateLanguage(language, HttpContext, Response);
            if (response.IsSuccess)
                return Redirect(path);
            else
                return View();
        }

        #endregion


        [AllowAnonymous]
        public async Task<IActionResult> SeedData()
        {
            await _service.SeedData();
            return Json(new object { });
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
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();

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
    }
}
