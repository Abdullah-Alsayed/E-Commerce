using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Category : BaseEntity
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
            Products = new HashSet<Product>();
        }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
