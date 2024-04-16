using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Bookings.Requests
{
    public record UpdateBookingRequest : BaseRequest
    {
        public Guid ProductID { get; set; }
        public Guid ColorID { get; set; }
        public Guid SizeID { get; set; }
    }
}
