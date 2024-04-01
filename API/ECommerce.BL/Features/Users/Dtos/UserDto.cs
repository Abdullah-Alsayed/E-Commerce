using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Users.Dtos
{
    public record UserDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Value { get; set; }
    }
}
