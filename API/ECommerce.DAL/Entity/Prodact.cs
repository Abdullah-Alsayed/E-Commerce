using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL.Entity
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
            ProductPhotos = new HashSet<ProductPhoto>();
            Favorites = new HashSet<Favorite>();
            Reviews = new HashSet<Review>();
        }

        public Guid BrandID { get; set; }
        public Guid UnitID { get; set; }

        [Required]
        public Guid SubCategoryID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required, Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, double.MaxValue)]
        public double DiscountLabel { get; set; }

        public virtual SubCategory SubCategory { get; set; } = new SubCategory();
        public virtual Category Category { get; set; } = new Category();
        public virtual Brand Brand { get; set; } = new Brand();
        public virtual Unit Unit { get; set; } = new Unit();

        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
