// Ignore Spelling: Barcode

using System;
using System.Collections.Generic;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.SubCategories.Dtos;
using ECommerce.BLL.Features.Tags.Dtos;

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
        public string Code { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double DiscountLabel { get; set; }

        public BrandDto Brand { get; set; }
        public SubCategoryDto SubCategory { get; set; }
        public UnitDto Unit { get; set; }
        public CategoryDto Category { get; set; }
    }
}
