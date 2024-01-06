using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class ProductOrder
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Order Order { get; set; } = new Order();
        public virtual Product Product { get; set; } = new Product();
    }
}
