using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Feedbacks.Requests
{
    public record UpdateFeedbackRequest : BaseRequest
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
