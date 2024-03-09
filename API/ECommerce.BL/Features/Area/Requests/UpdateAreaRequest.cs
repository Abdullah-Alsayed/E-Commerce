using System;

namespace ECommerce.BLL.Features.Area.Requests
{
    public class UpdateAreaRequest
    {
        public Guid ID { get; set; }
        public Guid GovernorateID { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }
    }
}
