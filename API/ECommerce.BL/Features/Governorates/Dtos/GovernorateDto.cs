using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Governorates.Dtos
{
    public record GovernorateDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
