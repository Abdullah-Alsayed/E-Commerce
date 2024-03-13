using System;

namespace ECommerce.BLL.Features.Carts.Requests
{
    public record CreateCartRequest
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
