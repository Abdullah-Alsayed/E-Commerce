using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Categories.Dtos
{
    public record CategoryDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string PhotoPath { get; set; }
    }
}
