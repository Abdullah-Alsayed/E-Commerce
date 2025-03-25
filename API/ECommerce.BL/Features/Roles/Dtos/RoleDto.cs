using ECommerce.BLL.DTO;
using ECommerce.Core.Enums;

namespace ECommerce.BLL.Features.Roles.Dtos
{
    public record RoleDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public RoleTypeEnum RoleType { get; set; } = RoleTypeEnum.User;
    }
}
