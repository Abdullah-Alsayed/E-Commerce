using System.Collections.Generic;
using System.Security.Claims;
using ECommerce.Core.PermissionsClaims;

namespace ECommerce.BLL.Features.Roles.Dtos
{
    public class AllClaimsDto
    {
        public string Key { get; set; }
        public List<ClaimDto> Claims { get; set; }
    }
}
