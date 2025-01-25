using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Areas.Dtos
{
    public record AreaDto : BaseEntityDto
    {
        public Guid GovernorateID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
