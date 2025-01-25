using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Units.Requests
{
    public record UpdateUnitRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
