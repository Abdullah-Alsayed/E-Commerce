using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.BLL.Features.Users.Filter
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler() { }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement
        )
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c =>
                c.Type == Constants.Permission && c.Value == requirement.Permission
            );

            if (canAccess)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
