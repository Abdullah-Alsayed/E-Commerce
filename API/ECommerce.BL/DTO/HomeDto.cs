using System.Collections.Generic;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.DTO
{
    public class HomeDto
    {
        public Setting Setting { get; set; }
        public IEnumerable<SliderPhoto> SliderPhotos { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductReview> Reviews { get; set; }
        public IEnumerable<ProductPhoto> ProductPhotos { get; set; }
    }
}
