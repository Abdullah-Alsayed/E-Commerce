using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Account.Dtos;
using ECommerce.BLL.Features.Account.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.MailServices;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMailServicies _mailServices;
        private readonly JWTHelpers _jwt;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IMailServicies mailServices,
            JWTHelpers jwt
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _jwt = jwt;
        }

        public async Task<User> FindUserByIDAsync(string UserID)
        {
            return await _userManager.FindByIdAsync(UserID);
        }

        public string GetUserID(ClaimsPrincipal user)
        {
            //return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return _userManager.GetUserId(user);
        }

        public string GetUserName(ClaimsPrincipal user)
        {
            return _userManager.GetUserName(user);
        }

        public string GetRoleID(ClaimsPrincipal user)
        {
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;
            Claim claim = identity.FindFirst(c => c.Type == "RoleID");
            return claim != null ? claim.Value : null;
        }

        public async Task<User> FindUserByNameAsync(string Name)
        {
            return await _userManager.FindByNameAsync(Name);
        }

        public async Task<User> FindUserByEmailAsync(string Email)
        {
            User Result = await _userManager.FindByEmailAsync(Email);
            return Result;
        }

        public async Task SendConfirmEmailAsync(User user)
        {
            string Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] EncodingToken = Encoding.UTF8.GetBytes(Token);
            string newToken = WebEncoders.Base64UrlEncode(EncodingToken);

            string confirmLink =
                $"https://localhost:44392/Account/ConfirmEmail?ID={user.Id}&Tocken={newToken}";
            string link = "<a href=\"" + confirmLink + "\">Confirm registration</a>";
            //mailServices.SendMail(user.Email, user.FirstName, txt, link, subject);
        }

        public Task ForgotPasswordAsync(object Entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> LoginAsync(LoginRequest request)
        {
            var user = _userManager.Users.FirstOrDefault(x =>
                x.UserName == request.UserName.ToLower()
                || x.Email == request.UserName.ToLower()
                || x.PhoneNumber == request.UserName
            );
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    request.Password,
                    request.RememberMe,
                    false
                );

                if (result.Succeeded)
                {
                    var token = await CreateJwtToken(user);
                    var rolesList = await _userManager.GetRolesAsync(user);
                    return new BaseResponse<LoginDto>
                    {
                        IsSuccess = result.Succeeded,
                        Message = Constants.MessageKeys.Success,
                        Result = new LoginDto
                        {
                            Email = user.Email,
                            ExpiresOn = token.ValidTo,
                            IsAuthenticated = true,
                            Roles = rolesList.ToList(),
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Username = user.UserName
                        }
                    };
                }
                else
                    return new BaseResponse<LoginDto>
                    {
                        IsSuccess = false,
                        Message = Constants.Errors.LoginFiled
                    };
            }
            else
            {
                return new BaseResponse<LoginDto>
                {
                    IsSuccess = false,
                    Message = Constants.Errors.LoginFiled
                };
            }
        }

        public async Task LoginAsync(User user, bool RememberMe)
        {
            await _signInManager.SignInAsync(user, RememberMe);
        }

        public async Task LogOffAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task ResetPasswordAsync(string Code)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePasswordAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<CreateUserDto>> CreateUserAsync(
            User user,
            string password,
            string userId
        )
        {
            BaseResponse result = new();
            var token = new JwtSecurityToken();
            BaseResponse userValidationResult = await UserValidation(user, result);
            if (userValidationResult.IsSuccess)
            {
                IdentityResult identityResult = await _userManager.CreateAsync(user, password);
                if (identityResult.Succeeded && !string.IsNullOrEmpty(userId))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Constants.Roles.User);
                    await LoginAsync(user, true);
                    token = await CreateJwtToken(user);
                }

                return new BaseResponse<CreateUserDto>
                {
                    IsSuccess = identityResult.Succeeded,
                    Message = identityResult.Succeeded
                        ? Constants.MessageKeys.Success
                        : string.Join(",", identityResult.Errors.Select(x => x.Description)),
                    Result =
                        (identityResult.Succeeded && !string.IsNullOrEmpty(userId))
                            ? new CreateUserDto
                            {
                                Email = user.Email,
                                ExpiresOn = token.ValidTo,
                                IsAuthenticated = true,
                                Roles = new List<string> { Constants.Roles.User },
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                Username = user.UserName
                            }
                            : null
                };
            }
            else
            {
                return new BaseResponse<CreateUserDto>
                {
                    IsSuccess = userValidationResult.IsSuccess,
                    Message = userValidationResult.Message
                };
            }
        }

        public async Task<bool> EmailExistAsync(string email)
        {
            bool Result = await FindUserByEmailAsync(email) != null;
            return Result;
        }

        public async Task<bool> UserNameExistAsync(string userName)
        {
            bool Result = await FindUserByNameAsync(userName) != null;
            return Result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User User, string Token)
        {
            byte[] newToken = WebEncoders.Base64UrlDecode(Token);
            string encodeToken = Encoding.UTF8.GetString(newToken);
            return await _userManager.ConfirmEmailAsync(User, encodeToken);
        }

        public async Task<bool> IsConfirmedAsync(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public bool IsAuthenticated(ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated;
        }

        public bool PhoneExist(string PhoneNumber)
        {
            bool Result = _context.Users.Where(p => p.PhoneNumber == PhoneNumber).Any();
            return Result;
        }

        #region helpers
        private async Task<BaseResponse> UserValidation(User user, BaseResponse result)
        {
            bool emailExist = await EmailExistAsync(user.Email);
            if (emailExist)
            {
                result.IsSuccess = false;
                result.Message = Constants.Errors.Emailexists;
                return result;
            }

            bool userNameExist = await UserNameExistAsync(user.UserName);
            if (userNameExist)
            {
                result.IsSuccess = false;
                result.Message = Constants.Errors.UserNameExists;
                return result;
            }
            bool phoneExists = PhoneExist(user.PhoneNumber);

            if (phoneExists)
            {
                result.IsSuccess = false;
                result.Message = Constants.Errors.PhoneNumbeExists;
                return result;
            }

            result.IsSuccess = true;
            return result;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> roleClaims = new();

            foreach (string role in roles)
            {
                var rolex = await _roleManager.FindByNameAsync(role);
                var roleClaimsx = await _roleManager.GetClaimsAsync(rolex);
                foreach (var Claim in roleClaimsx)
                    userClaims.Add(Claim);
                roleClaims.Add(new Claim("roles", role));
            }

            IEnumerable<Claim> claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("ID", user.Id),
                new Claim("Language", user.Language),
                new Claim("UserName", user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}")
            }
                .Union(userClaims)
                .Union(roleClaims);

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwt.Key));
            SigningCredentials signingCredentials =
                new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken =
                new(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                    signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }
        #endregion
    }
}
