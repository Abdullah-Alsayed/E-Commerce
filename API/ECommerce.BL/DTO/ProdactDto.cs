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
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public int BrandID { get; set; }
        public int UnitID { get; set; }
        public int ColorID { get; set; }

        [Required]
        public int SubCategoryID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }

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
