using System;
using System.Collections.Generic;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Stocks.Requests
{
    public record UpdateStockRequest : BaseRequest
    {
        public Guid ProductID { get; set; }
        public Guid VendorID { get; set; }
        public int Quantity { get; set; }
        public List<StockItemDto> Size { get; set; }
        public List<StockItemDto> Color { get; set; }
    }
}
