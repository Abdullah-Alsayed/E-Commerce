using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class ProductColor
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ColorID { get; set; }
        public Guid ProductID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
    }
}
