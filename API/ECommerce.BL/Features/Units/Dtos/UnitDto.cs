using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Units.Dtos
{
    public record UnitDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
