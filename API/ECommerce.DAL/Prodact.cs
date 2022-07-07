using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class Prodact
    {
        public Prodact()
        {
            Favorites     = new HashSet<Favorite>();
            prodactOrders = new HashSet<ProdactOrder>();
            ProdactImgs   = new HashSet<ProdactImg>();
            Reviews       = new HashSet<Review>();
        }
        public int ID { get; set; }

        [Required , StringLength(150)]
        public string Title { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public int BrandID { get; set; }
        public int UnitsID { get; set; }
        public int ColorID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey("User") , Required , StringLength(450)]
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual Brand brand { get; set; }
        public virtual Unit unit { get; set; }
        public virtual Color color { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<ProdactOrder> prodactOrders { get; set; }
        public virtual ICollection<ProdactImg> ProdactImgs { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }


    }
}
