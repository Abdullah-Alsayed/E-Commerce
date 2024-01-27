using System;

namespace ECommerce.BLL.Futures.Governorates.Requests
{
    public class UpdateGovernorateRequest
    {
        public Guid ID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
