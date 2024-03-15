using System;
using System.Collections.Generic;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Products.Dtos
{
    public record ProductDto : BaseEntityDto
    {
        public Guid? BrandID { get; set; }
        public Guid? SubCategoryID { get; set; }
        public Guid UnitID { get; set; }
        public Guid CategoryID { get; set; }
        public List<string> ProductPhotos { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double DiscountLabel { get; set; }
    }
}
