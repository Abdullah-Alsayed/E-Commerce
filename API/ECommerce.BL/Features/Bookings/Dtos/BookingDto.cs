using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Bookings.Dtos
{
    public record BookingDto : BaseEntityDto
    {
        public Guid ProductID { get; set; }
        public Guid ColorID { get; set; }
        public Guid SizeID { get; set; }
        public bool IsNotified { get; set; }
    }
}
