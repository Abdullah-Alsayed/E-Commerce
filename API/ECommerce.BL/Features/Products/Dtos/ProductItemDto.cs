using System.Collections.Generic;
using ECommerce.BLL.Features.Colors.Dtos;
using ECommerce.BLL.Features.Sizes.Dtos;

namespace ECommerce.BLL.Features.Products.Dtos
{
    public class ProductItemDto
    {
        public ProductDto Product { get; set; }
        public List<ProductColorDto> ProductColors { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; }
    }

    public class ProductSizeDto
    {
        public SizeDto Size { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductColorDto
    {
        public ColorDto Color { get; set; }
        public int Quantity { get; set; }
    }
}
