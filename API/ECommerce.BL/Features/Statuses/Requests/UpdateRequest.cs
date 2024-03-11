using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Statuses.Requests
{
    public record UpdateStatusRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
