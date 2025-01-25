using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Feedbacks.Dtos
{
    public record FeedbackDto : BaseEntityDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
