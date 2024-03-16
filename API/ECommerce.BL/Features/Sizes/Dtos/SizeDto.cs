using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Sizes.Dtos
{
    public record SizeDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
