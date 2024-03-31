using Microsoft.AspNetCore.Authorization;

namespace ECommerce.BLL.Features.Users.Filter
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission) => Permission = permission;
    }
}
