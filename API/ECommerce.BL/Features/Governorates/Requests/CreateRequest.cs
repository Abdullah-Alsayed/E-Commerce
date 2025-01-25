using System.ComponentModel;

namespace ECommerce.BLL.Features.Governorates.Requests
{
    public record CreateGovernorateRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
