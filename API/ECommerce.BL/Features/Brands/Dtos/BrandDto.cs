using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Brands.Dtos
{
    public record BrandDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string PhotoPath { get; set; }
    }
}
