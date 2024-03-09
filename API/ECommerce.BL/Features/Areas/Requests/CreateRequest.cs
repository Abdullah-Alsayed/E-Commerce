using System;

namespace ECommerce.BLL.Features.Areas.Requests
{
    public record CreateAreaRequest
    {
        public Guid GovernorateID { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }
    }
}
