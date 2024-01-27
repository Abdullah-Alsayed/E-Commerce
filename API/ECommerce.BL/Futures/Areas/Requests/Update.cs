using System;

namespace ECommerce.BLL.Futures.Areas.Requests
{
    public class UpdateAreaRequest
    {
        public Guid ID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
