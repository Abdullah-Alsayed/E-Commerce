using System;

namespace ECommerce.BLL.Features.Reviews.Requests
{
    public record CreateReviewRequest
    {
        public Guid ProductID { get; set; }
        public string Review { get; set; }
        public int Rate { get; set; }
    }
}
