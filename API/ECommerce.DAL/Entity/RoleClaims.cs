using System;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class RoleClaims : IdentityRoleClaim<Guid>
    {
        public string Module { get; set; }
        public string Operation { get; set; }
    }
}
