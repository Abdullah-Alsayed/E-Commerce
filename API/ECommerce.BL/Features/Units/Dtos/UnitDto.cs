using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Tags.Dtos
{
    public record UnitDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
