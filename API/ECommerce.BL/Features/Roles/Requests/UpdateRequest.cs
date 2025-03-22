using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public record UpdateRoleRequest : BaseRequest
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
    }
}
