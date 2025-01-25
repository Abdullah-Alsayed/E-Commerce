using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.Helpers
{
    public static class AuthHelper
    {
        public static string GetClaimValue(ClaimsPrincipal user, string type)
        {
            if (user.Identity is ClaimsIdentity identity)
            {
                var claim = identity.FindFirst(c => c.Type == type);
                if (claim != null)
                    return claim.Value;
            }

            return null;
        }

        public static async Task Authentication(
            HttpContext httpContext,
            string roleName,
            string userRole,
            string language,
            string userPhoto,
            string RoleId,
            string email,
            string userName,
            string userId
        )
        {
            var claims = new List<Claim>
            {
                new Claim("UserID", userId),
                new Claim("Name", userName),
                new Claim("Email", email),
                new Claim("RoleID", RoleId),
                new Claim("UserRole", userRole),
                new Claim("UserPhoto", userPhoto),
                new Claim("Language", language),
                new Claim("RoleName", roleName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            var authProperties = new AuthenticationProperties { AllowRefresh = true, };
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        public static async Task UpdateLanguage(
            string language,
            HttpContext httpContext,
            HttpResponse response
        )
        {
            var currentUser = httpContext.User;
            var claimsIdentity = currentUser.Identity as ClaimsIdentity;

            // Update the claims with the new language
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("Language"));

            claimsIdentity.AddClaim(new Claim("Language", language ?? ""));

            // Update the authentication cookie with the new claims
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { AllowRefresh = true }
            );
            response.Cookies.Append(
                "PreferredLanguage",
                language,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }

        public static string Premission(string entity, string action)
        {
            return $"{action}{entity}";
        }

        public static bool CanAccess(List<string> permissions, string action)
        {
            //  return permissions.Contains(action);
            return true;
        }
    }
}
