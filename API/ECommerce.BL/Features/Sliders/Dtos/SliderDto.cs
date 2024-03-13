using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Sliders.Dtos
{
    public record SliderDto : BaseEntityDto
    {
        public string TitleAR { get; set; }
        public string TitleEN { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
    }
}
