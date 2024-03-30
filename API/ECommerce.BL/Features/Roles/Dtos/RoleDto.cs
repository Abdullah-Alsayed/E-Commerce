using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Roles.Dtos
{
    public record RoleDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
