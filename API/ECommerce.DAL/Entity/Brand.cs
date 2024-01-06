using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECommerce.DAL.Entity
{
    public class Brand : BaseEntity
    {
        public Brand() => Products = new HashSet<Product>();

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
