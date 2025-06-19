// Ignore Spelling: Barcode

using System;
using System.Collections.Generic;
using ECommerce.BLL.Features.Products.Dtos;

namespace ECommerce.BLL.Features.Products.Requests
{
    public record CreateProductRequest
    {
        public Guid? BrandID { get; set; }
        public Guid UnitID { get; set; }
        public Guid? SubCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public List<Guid> CollectionIDs { get; set; }
        public List<String> TagIDs { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public bool AllStockOut { get; set; }
        public List<ProductPhotoDto> Photos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double DiscountLabel { get; set; }
    }
}
