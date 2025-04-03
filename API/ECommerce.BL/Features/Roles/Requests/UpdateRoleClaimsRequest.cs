using System;
using System.Collections.Generic;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public class UpdateClaimsRequest
    {
        public Guid ID { get; set; }
        public List<string> Claims { get; set; }
    }
}
