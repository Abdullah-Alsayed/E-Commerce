using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Users.Dtos;

namespace ECommerce.BLL.Features.Feedbacks.Dtos
{
    public record FeedbackDto : BaseEntityDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }

        public UserDto User { get; set; }
    }
}
