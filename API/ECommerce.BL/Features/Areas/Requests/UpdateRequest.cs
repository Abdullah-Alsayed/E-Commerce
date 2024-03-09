using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Areas.Requests
{
    public record UpdateAreaRequest : BaseRequest
    {
        public Guid GovernorateID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
