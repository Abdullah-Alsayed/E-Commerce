using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL.Entity
{
    public class SubCategory : BaseEntity
    {
        public SubCategory() => Products = new HashSet<Product>();

        [Required]
        public Guid CategoryID { get; set; }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }

        public virtual Category Category { get; set; } = new Category();
        public virtual ICollection<Product> Products { get; set; }
    }
}
