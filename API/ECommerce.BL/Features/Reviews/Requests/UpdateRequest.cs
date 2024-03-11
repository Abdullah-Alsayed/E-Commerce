using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Reviews.Requests
{
    public record UpdateReviewRequest : BaseRequest
    {
        public string Review { get; set; }
        public int Rate { get; set; }
    }
}
