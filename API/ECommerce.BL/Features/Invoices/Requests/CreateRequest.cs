using System;

namespace ECommerce.BLL.Features.Invoices.Requests
{
    public record CreateInvoiceRequest
    {
        public Guid OrderID { get; set; }
    }
}
