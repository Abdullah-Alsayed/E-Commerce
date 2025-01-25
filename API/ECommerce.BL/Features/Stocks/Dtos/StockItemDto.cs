using System;

namespace ECommerce.BLL.Features.Stocks.Dtos
{
    public record StockItemDto
    {
        public Guid ID { get; set; }
        public int Quantity { get; set; }
    }
}
