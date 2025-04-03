using System;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class UserClaims : IdentityUserClaim<Guid>
    {
        public string Module { get; set; }
        public string Operation { get; set; }
    }
}
