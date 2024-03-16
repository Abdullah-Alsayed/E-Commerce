using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Invoices.Dtos
{
    public record InvoiceDto : BaseEntityDto
    {
        public Guid OrderID { get; set; }
    }
}
