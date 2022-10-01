using ECommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.DTO
{
    public class ProdactDto
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
        public IEnumerable<ProdactImg> Img { get; set; }
       

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
        //public virtual ICollection<ProdactOrder> prodactOrders { get; set; }
        //public virtual ICollection<ProdactImg> ProdactImgs { get; set; }
        //public virtual ICollection<Review> Reviews { get; set; }
    }
}
