using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.Services.User;

public class UserContext : IUserContext
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public (bool Exist, string Value) UserId
    {
        get
        {
            if (
                _httpContextAccessor is not { HttpContext: null }
                && _httpContextAccessor.HttpContext is not { User: null }
                && _httpContextAccessor.HttpContext.User is not { Identity: null }
                && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
            )
            {
                var userId = _httpContextAccessor
                    .HttpContext.User.Claims.FirstOrDefault(x =>
                        x.Type == ClaimTypes.NameIdentifier
                    )
                    ?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    userId = _httpContextAccessor.HttpContext.User.Identity.Name;
                }
                return !string.IsNullOrEmpty(userId) ? new(true, userId) : new(false, userId);
            }
            else
            {
                return (false, null);
            }
        }
    }

    public (bool Exist, string Value) UserName
    {
        get
        {
            if (
                _httpContextAccessor is not { HttpContext: null }
                && _httpContextAccessor.HttpContext is not { User: null }
                && _httpContextAccessor.HttpContext.User is not { Identity: null }
                && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
            )
            {
                var userName = _httpContextAccessor
                    .HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.Claims.ID)
                    ?.Value;
                return !string.IsNullOrEmpty(userName) ? new(true, userName) : new(false, userName);
            }
            else
            {
                return (false, null);
            }
        }
    }

    public (bool Exist, string Value) Language
    {
        get
        {
            if (
                _httpContextAccessor is not { HttpContext: null }
                && _httpContextAccessor.HttpContext is not { User: null }
                && _httpContextAccessor.HttpContext.User is not { Identity: null }
                && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
            )
            {
                var _language = _httpContextAccessor
                    .HttpContext.User.Claims.FirstOrDefault(x =>
                        x.Type == Constants.Claims.Language
                    )
                    ?.Value;

                return !string.IsNullOrEmpty(_language) ? (true, _language) : (false, string.Empty);
            }
            else
                return (false, string.Empty);
        }
    }
}
