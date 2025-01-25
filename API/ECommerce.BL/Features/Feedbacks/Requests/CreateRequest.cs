namespace ECommerce.BLL.Features.Feedbacks.Requests
{
    public record CreateFeedbackRequest
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
