// Ignore Spelling: Barcode

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
            Favorites = new HashSet<Favorite>();
            Reviews = new HashSet<ProductReview>();
            ProductTags = new HashSet<ProductTag>();
            ProductPhotos = new HashSet<ProductPhoto>();
        }

        public Guid? BrandID { get; set; }

        [Required]
        public Guid UnitID { get; set; }

        public Guid? SubCategoryID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required, Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, double.MaxValue)]
        public double DiscountLabel { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Barcode { get; set; }
        public bool AllStockOut { get; set; }

        public virtual SubCategory SubCategory { get; set; }
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Unit Unit { get; set; }

        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Stock> ProductStocks { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
