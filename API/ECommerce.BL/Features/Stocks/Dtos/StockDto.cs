using System;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Products.Dtos;
using ECommerce.BLL.Features.Vendors.Dtos;

namespace ECommerce.BLL.Features.Stocks.Dtos
{
    public record StockDto : BaseEntityDto
    {
        public Guid ProductID { get; set; }
        public Guid VendorID { get; set; }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }
        public VendorDto Vendor { get; set; }
    }
}
