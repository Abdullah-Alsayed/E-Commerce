using System;
using System.Collections.Generic;
using ECommerce.BLL.Features.Carts.Dtos;

namespace ECommerce.BLL.Features.Orders.Requests
{
    public record CreateOrderRequest
    {
        public Guid AreaID { get; set; }
        public Guid GovernorateID { get; set; }
        public Guid? VoucherID { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsOffLine { get; set; }
        public List<ProdcuOrder> Products { get; set; }
    }

    public record ProdcuOrder
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
