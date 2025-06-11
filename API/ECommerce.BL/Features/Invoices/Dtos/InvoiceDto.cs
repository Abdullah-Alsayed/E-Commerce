using System;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Orders.Dtos;

namespace ECommerce.BLL.Features.Invoices.Dtos
{
    public record InvoiceDto : BaseEntityDto
    {
        public Guid OrderID { get; set; }
        public bool IsReturn { get; set; }

        public OrderDto Order { get; set; }
    }
}
