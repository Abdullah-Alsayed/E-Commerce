using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Products.Requests
{
    public record CreateProductRequest
    {
        public Guid? BrandID { get; set; }
        public Guid UnitID { get; set; }
        public Guid? SubCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double DiscountLabel { get; set; }
        public List<IFormFile> FormFiles { get; set; }
    }
}
