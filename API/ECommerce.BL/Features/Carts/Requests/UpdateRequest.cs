using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Carts.Requests
{
    public record UpdateCartRequest : BaseRequest
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
