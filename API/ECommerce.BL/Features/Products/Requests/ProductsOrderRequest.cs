using System;

namespace ECommerce.BLL.Features.Products.Requests
{
    public class ProductsOrderRequest
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
