using System;
using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public class AddUserToRoleRequest
    {
        public Guid UserID { get; set; }
        public List<Guid> RoleIDs { get; set; }
    }
}
