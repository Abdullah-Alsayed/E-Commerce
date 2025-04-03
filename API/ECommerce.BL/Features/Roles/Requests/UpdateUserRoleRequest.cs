using System;
using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests;

public class UpdateUserRoleRequest
{
    public Guid UserID { get; set; }
    public List<Guid> RoleIDs { get; set; }
}
