using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.SubCategories.Dtos;
using ECommerce.BLL.Features.Tags.Dtos;

namespace ECommerce.Portal.ViewModels
{
    public class CreateProductViewModel
    {
        public List<CategoryDto> Categories { get; set; } = new();
        public List<UnitDto> Units { get; set; } = new();
        public List<BrandDto> Brands { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
    }
}
