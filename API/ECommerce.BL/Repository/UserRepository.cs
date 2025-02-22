using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Azure.Core;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.BLL.Features.Users.Requests;
using ECommerce.BLL.Features.Users.Services;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Helpers;
using ECommerce.Core.Services.MailServices;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Twilio.Http;

namespace ECommerce.BLL.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<UserRepository> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMailServices _mailServices;
        private readonly JWTHelpers _jwt;

        public UserRepository(
            ApplicationDbContext context,
            IStringLocalizer<UserRepository> localizer,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IMailServices mailServices,
            JWTHelpers jwt
        )
            : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailServices = mailServices;
            _jwt = jwt;
            _localizer = localizer;
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

        public async Task<bool> SendConfirmEmailAsync(User user)
        {
            string Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            byte[] EncodingToken = Encoding.UTF8.GetBytes(Token);
            string newToken = WebEncoders.Base64UrlEncode(EncodingToken);
            string confirmLink =
                $"{Constants.HostName}/api/User/ConfirmEmail?ID={user.Id}&Token={newToken}";
            string link = "<a href=\"" + confirmLink + "\">Confirm registration</a>";

            return await _mailServices.SendAsync(
                new EmailDto
                {
                    Email = user.Email,
                    Body = link,
                    Subject = _localizer[Constants.Message.ConfirmEmail].ToString()
                }
            );
        }

        public async Task<BaseResponse> ConfirmEmailAsync(string userID, string token)
        {
            var user = await GetUser(userID);
            if (user != null)
            {
                byte[] newToken = WebEncoders.Base64UrlDecode(token);
                string encodeToken = Encoding.UTF8.GetString(newToken);
                var result = await _userManager.ConfirmEmailAsync(user, encodeToken);
                return new BaseResponse
                {
                    IsSuccess = result.Succeeded,
                    Message = result.Succeeded
                        ? _localizer[Constants.MessageKeys.Success].ToString()
                        : _localizer[Constants.MessageKeys.Fail].ToString()
                };
            }
            else
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = Constants.MessageKeys.UserNotFound
                };
            }
        }

        public Task ForgotPasswordAsync(object Entity)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x =>
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
                            UserName = user.UserName,
                            FullName = $"{user.FirstName} {user.LastName}",
                            Photo = user.Photo
                        }
                    };
                }
                else
                    return new BaseResponse<LoginDto>
                    {
                        IsSuccess = false,
                        Message = _localizer[Constants.MessageKeys.LoginFiled].ToString()
                    };
            }
            else
            {
                return new BaseResponse<LoginDto>
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.LoginFiled].ToString()
                };
            }
        }

        public async Task<BaseResponse> WebLoginAsync(LoginRequest request, HttpContext httpContext)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x =>
                x.UserName.ToLower() == request.UserName.ToLower()
                || x.Email.ToLower() == request.UserName.ToLower()
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
                    var claims = await GetUserClaims(user);

                    // Merge all claims while ensuring uniqueness by Type
                    var authProperties = new AuthenticationProperties { AllowRefresh = true };

                    // Create a new ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );

                    await SetDataOnCookie(httpContext, user);

                    // Sign in the user with the updated claims
                    await httpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return new BaseResponse<LoginDto>
                    {
                        IsSuccess = result.Succeeded,
                        Message = Constants.MessageKeys.Success,
                    };
                }
                else
                    return new BaseResponse<LoginDto>
                    {
                        IsSuccess = false,
                        Message = _localizer[Constants.MessageKeys.LoginFiled].ToString()
                    };
            }
            else
            {
                return new BaseResponse<LoginDto>
                {
                    IsSuccess = false,
                    Message = _localizer[Constants.MessageKeys.LoginFiled].ToString()
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
            // Generate unique username
            user.UserName = await GenerateUniqueUsernameAsync(user.FirstName, user.LastName);
            BaseResponse userValidationResult = await UserValidation(user, result);
            if (userValidationResult.IsSuccess)
            {
                IdentityResult identityResult = await _userManager.CreateAsync(user, password);
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

        public async Task<BaseResponse<CreateUserDto>> RegisterUserAsync(
            User user,
            string password,
            string userId
        )
        {
            BaseResponse result = new();
            var token = new JwtSecurityToken();
            // Generate unique username
            user.UserName = await GenerateUniqueUsernameAsync(user.FirstName, user.LastName);
            BaseResponse userValidationResult = await UserValidation(user, result);
            if (userValidationResult.IsSuccess)
            {
                IdentityResult identityResult = await _userManager.CreateAsync(user, password);
                if (identityResult.Succeeded && !string.IsNullOrEmpty(userId))
                {
                    if (identityResult.Succeeded && !string.IsNullOrEmpty(userId))
                        await _userManager.AddToRoleAsync(user, user.RoleId);

                    await LoginAsync(user, true);
                    token = await CreateJwtToken(user);
                    await SendConfirmEmailAsync(user);
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

        private async Task<string> GenerateUniqueUsernameAsync(string firstName, string lastName)
        {
            // Ensure first and last names are not null or empty
            firstName = string.IsNullOrWhiteSpace(firstName) ? "" : firstName.Trim().ToLower();
            lastName = string.IsNullOrWhiteSpace(lastName) ? "" : lastName.Trim().ToLower();

            // Base username logic
            string baseUsername;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                baseUsername = $"{firstName}.{lastName}";
            else if (!string.IsNullOrEmpty(firstName))
                baseUsername = firstName;
            else if (!string.IsNullOrEmpty(lastName))
                baseUsername = lastName;
            else
                baseUsername = "user";

            string username = baseUsername;
            int counter = 1;

            while (await _context.Users.AnyAsync(u => u.UserName == username))
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }
            return username;
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

        public async Task<BaseResponse> ChangePassword(
            ChangePasswordUserRequest request,
            string userId
        )
        {
            try
            {
                var user = await GetUser(userId);

                if (user != null)
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(
                        user,
                        request.OldPassword
                    );
                    if (isPasswordValid)
                    {
                        var result = await _userManager.ChangePasswordAsync(
                            user,
                            request.OldPassword,
                            request.NewPassword
                        );

                        return new BaseResponse
                        {
                            IsSuccess = result.Succeeded,
                            Message = result.Succeeded
                                ? Constants.MessageKeys.Success
                                : Constants.MessageKeys.Fail
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Message = Constants.MessageKeys.PasswordIsWrong
                        };
                    }
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = Constants.MessageKeys.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> ChangeUserPassword(ChangeUserPasswordRequest request)
        {
            try
            {
                var user = await GetUser(request.ID);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(
                        user,
                        token,
                        request.NewPassword
                    );

                    return new BaseResponse
                    {
                        IsSuccess = result.Succeeded,
                        Message = result.Succeeded
                            ? _localizer[Constants.MessageKeys.Success].ToString()
                            : _localizer[Constants.MessageKeys.Fail].ToString()
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = Constants.MessageKeys.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> ForgotPassword(
            ForgotPasswordUserRequest request,
            string userId
        )
        {
            try
            {
                var user = await GetUserByEmail(request.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    if (!string.IsNullOrEmpty(token))
                    {
                        var url =
                            $"{Constants.HostName}/api/User/ResetPassword?token={HttpUtility.UrlEncode(token)}&email={user.Email}";

                        var result = await _mailServices.SendAsync(
                            new EmailDto
                            {
                                Body = $"{_localizer[Constants.MessageKeys.ForgotPassword]}: {url}",
                                Email = user.Email,
                                Subject = _localizer[Constants.MessageKeys.RestPassword].ToString(),
                            }
                        );

                        return new BaseResponse
                        {
                            IsSuccess = result,
                            Message = result
                                ? _localizer[Constants.MessageKeys.Success].ToString()
                                : _localizer[Constants.MessageKeys.Fail].ToString()
                        };
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            IsSuccess = false,
                            Message = Constants.MessageKeys.PasswordIsWrong
                        };
                    }
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = Constants.MessageKeys.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> ResetPassword(
            ResetPasswordUserRequest request,
            string userId
        )
        {
            try
            {
                var user = await GetUserByEmail(request.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(
                        user,
                        HttpUtility.UrlDecode(request.Token),
                        request.NewPassword
                    );
                    return new BaseResponse
                    {
                        IsSuccess = result.Succeeded,
                        Message = result.Succeeded
                            ? Constants.MessageKeys.Success
                            : _localizer[result.Errors.FirstOrDefault().Code].ToString()
                    };
                }
                else
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = Constants.MessageKeys.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<User> DeleteAsync(string UserID, string token)
        {
            try
            {
                var user = await GetUser(UserID);
                //var result = _userManager.RemoveAuthenticationTokenAsync();
                await _context.TokenExpired.AddAsync(
                    new TokenExpired { Token = token, UserID = UserID }
                );
                return user;
            }
            catch (Exception ex)
            {
                await _context.ErrorLogs.AddAsync(
                    new ErrorLog
                    {
                        Source = ex.Source,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Operation = DAL.Enums.OperationTypeEnum.GetAll,
                        Entity = DAL.Enums.EntitiesEnum.User
                    }
                );
                await _context.SaveChangesAsync();
                return null;
            }
        }

        public bool IsTokenExperts(StringValues token)
        {
            return _context.TokenExpired.Any(x => x.Token == token);
        }

        public async Task<List<User>> GetAllAsync(GetAllUserRequest request)
        {
            try
            {
                var result = new List<User>();
                var query = _context
                    .Users.Include(x => x.Role)
                    .Where(u => !u.IsDeleted && u.UserName != Constants.System);

                if (!string.IsNullOrEmpty(request.SortBy))
                    query = OrderByDynamic(query, request.SortBy, request.IsDescending);

                if (
                    !string.IsNullOrEmpty(request.SearchFor)
                    && !string.IsNullOrEmpty(request.SearchBy)
                )
                    query = SearchDynamic(query, request.SearchBy, request.SearchFor);

                var total = await query.CountAsync();
                if (total > 0)
                {
                    query = ApplyPagination(request, query);
                    result = await query.AsNoTracking().ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                await _context.ErrorLogs.AddAsync(
                    new ErrorLog
                    {
                        Source = ex.Source,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Operation = DAL.Enums.OperationTypeEnum.GetAll,
                        Entity = DAL.Enums.EntitiesEnum.User
                    }
                );
                await _context.SaveChangesAsync();
                return new List<User>();
            }
        }

        public async Task<User> GetAsync(string userId)
        {
            try
            {
                var query = await _context
                    .Users.Include(x => x.Role)
                    .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == userId);

                return query;
            }
            catch (Exception ex)
            {
                await _context.ErrorLogs.AddAsync(
                    new ErrorLog
                    {
                        Source = ex.Source,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Operation = DAL.Enums.OperationTypeEnum.Get,
                        Entity = DAL.Enums.EntitiesEnum.User
                    }
                );
                await _context.SaveChangesAsync();
                return default;
            }
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
            bool phoneValid = GeneralHelpers.IsPhoneValid(user.PhoneNumber);

            if (phoneValid)
            {
                result.IsSuccess = false;
                result.Message = Constants.Errors.PhoneNumbeIsnotValid;
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
            IEnumerable<Claim> claims = await GetUserClaims(user);

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

        private async Task<IEnumerable<Claim>> GetUserClaims(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> roleClaims = new();

            foreach (string role in roles)
            {
                var rolex = await _roleManager.FindByNameAsync(role);
                var roleClaimsx = await _roleManager.GetClaimsAsync(rolex);
                foreach (var claim in roleClaimsx)
                    userClaims.Add(claim);

                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Constants.Claims.ID, user.Id),
                new Claim(Constants.Claims.Language, user.Language),
                new Claim(Constants.Claims.UserName, user.UserName),
                new Claim(Constants.Claims.FirstName, user.FirstName),
                new Claim(Constants.Claims.LastName, user.LastName),
                new Claim(Constants.Claims.FullName, $"{user.FirstName} {user.LastName}"),
                new Claim(Constants.Claims.UserPhoto, user.Photo)
            }
                .Concat(userClaims)
                .Concat(roleClaims)
                .Concat(new List<Claim> { new Claim("abdullah", "value") });

            return claims;
        }

        private async Task<User> GetUser(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        private async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x =>
                x.Email.ToLower() == email.ToLower()
            );
        }

        private IQueryable<Q> OrderByDynamic<Q>(
            IQueryable<Q> query,
            string orderByColumn,
            bool isDesc
        )
        {
            var QType = typeof(Q);
            orderByColumn =
                orderByColumn != "ID"
                    ? char.ToUpper(orderByColumn[0]) + orderByColumn.Substring(1)
                    : "UserName";
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? "Name");
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // this is the part p => p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(
                typeof(Queryable),
                isDesc ? "OrderByDescending" : "OrderBy",
                new Type[] { QType, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression)
            );

            return query.Provider.CreateQuery<Q>(resultExpression);
        }

        private async Task SetDataOnCookie(HttpContext httpContext, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = string.Join(",", roles);

            httpContext.Response.Cookies.Append(Constants.Claims.ID, user.Id);
            httpContext.Response.Cookies.Append(Constants.Claims.Language, user.Language);
            httpContext.Response.Cookies.Append(Constants.Claims.UserName, user.UserName);
            httpContext.Response.Cookies.Append(Constants.Claims.FirstName, user.FirstName);
            httpContext.Response.Cookies.Append(Constants.Claims.LastName, user.LastName);
            httpContext.Response.Cookies.Append(
                Constants.Claims.FullName,
                $"{user.FirstName} {user.LastName}"
            );
            httpContext.Response.Cookies.Append(Constants.Claims.UserPhoto, user.Photo);
            httpContext.Response.Cookies.Append(Constants.Claims.RoleName, roleName);
        }

        public async Task SeedData()
        {
            await DataSeeder.SeedData(_context, _roleManager, _userManager);
        }

        #endregion
    }
}
