using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class RoleClaims : IdentityRoleClaim<string>
    {
        public string Module { get; set; }
        public string Operation { get; set; }
    }
}
