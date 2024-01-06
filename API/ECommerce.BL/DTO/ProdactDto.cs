using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class ProductDto
    {
        public int ID { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required]
        public int Quantity { get; set; }
        public int Total { get; set; }

        [Required]
        public double Price { get; set; }
        public double Discount { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
        public IEnumerable<ProductPhoto> ProductPhotos { get; set; }

        public double TotalPrice()
        {
            return Total * Quantity;
        }

        //public virtual User User { get; set; }
        //public virtual Category Category { get; set; }
        //public virtual Brand brand { get; set; }
        //public virtual Unit Unit { get; set; }
        //public virtual Color color { get; set; }
        //public virtual SubCategory SubCategory { get; set; }
        //public virtual ICollection<Favorite> Favorites { get; set; }
        //public virtual ICollection<ProductOrder> prodactOrders { get; set; }
        //public virtual ICollection<ProductImg> ProductImgs { get; set; }
        //public virtual ICollection<Review> Reviews { get; set; }
    }
}
