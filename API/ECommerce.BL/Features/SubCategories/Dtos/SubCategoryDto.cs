using System;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Categories.Dtos;

namespace ECommerce.BLL.Features.SubCategories.Dtos
{
    public record SubCategoryDto : BaseEntityDto
    {
        public Guid CategoryID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string PhotoPath { get; set; }

        public CategoryDto Category { get; set; }
    }
}
