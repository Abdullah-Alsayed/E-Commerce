using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class ProductColor
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid ColorId { get; set; }
        public Guid ProductId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Count { get; set; }

        public virtual Color Color { get; set; } = new Color();
        public virtual Product Product { get; set; } = new Product();
    }
}
