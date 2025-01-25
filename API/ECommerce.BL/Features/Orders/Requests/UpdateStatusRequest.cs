using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Orders.Requests
{
    public record UpdateStatusOrderRequest : BaseRequest
    {
        public Guid StatusID { get; set; }
    }
}
