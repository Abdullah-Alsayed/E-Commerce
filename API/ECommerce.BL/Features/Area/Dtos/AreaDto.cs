using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Area.Dtos
{
    public class AreaDto : BaseEntityDto
    {
        public Guid ID { get; set; }
        public Guid GovernorateID { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }
    }
}
