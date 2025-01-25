using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests;

public class UpdateUserRoleRequest
{
    public string UserID { get; set; }
    public List<string> RoleIDs { get; set; }
}
