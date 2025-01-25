using System;

namespace ECommerce.BLL.Features.Bookings.Requests
{
    public record CreateBookingRequest
    {
        public Guid ProductID { get; set; }
        public Guid ColorID { get; set; }
        public Guid SizeID { get; set; }
    }
}
