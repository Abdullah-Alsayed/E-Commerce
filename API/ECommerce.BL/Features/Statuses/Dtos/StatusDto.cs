using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Statuses.Dtos
{
    public record StatusDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Order { get; set; }
    }
}
