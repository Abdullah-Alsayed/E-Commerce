using System;
using System.Collections.Generic;
using ECommerce.BLL.Features.Stocks.Dtos;

namespace ECommerce.BLL.Features.Stocks.Requests
{
    public record CreateStockRequest
    {
        public Guid ProductID { get; set; }
        public Guid VendorID { get; set; }
        public int Quantity { get; set; }
        public List<StockItemDto> Size { get; set; }
        public List<StockItemDto> Color { get; set; }
    }
}
