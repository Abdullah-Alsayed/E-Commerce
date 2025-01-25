using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Sizes.Requests
{
    public record UpdateSizeRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
