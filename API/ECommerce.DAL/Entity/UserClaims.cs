using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class UserClaims : IdentityUserClaim<string>
    {
        public string Module { get; set; }
        public string Operation { get; set; }
    }
}
