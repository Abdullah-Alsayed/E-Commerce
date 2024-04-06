using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public class UpdateRoleClaimsRequest
    {
        public string RoleID { get; set; }
        public List<string> Claims { get; set; }
    }
}
