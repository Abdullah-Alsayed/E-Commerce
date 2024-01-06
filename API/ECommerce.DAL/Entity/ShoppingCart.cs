using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class ShoppingCart : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
