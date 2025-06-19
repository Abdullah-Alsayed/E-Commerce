using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Colors.Dtos
{
    public record ColorDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Value { get; set; }
    }
}
