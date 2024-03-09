using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Governorates.Requests
{
    public record UpdateGovernorateRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
