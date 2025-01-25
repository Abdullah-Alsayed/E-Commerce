using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public class UpdateUserClaimsRequest
    {
        public string UserID { get; set; }
        public List<string> Claims { get; set; }
    }
}
