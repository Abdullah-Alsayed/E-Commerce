using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Roles.Requests
{
    public record CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
