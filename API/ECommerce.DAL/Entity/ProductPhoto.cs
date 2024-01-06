using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class ProductPhoto : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required, StringLength(255)]
        public string PhotoPath { get; set; }

        public virtual Product Product { get; set; } = new Product();
    }
}
