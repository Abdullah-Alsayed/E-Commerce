using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Colors.Requests
{
    public record UpdateColorRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Value { get; set; }
    }
}
