using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity;

public class ProductSize
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public Guid SizeId { get; set; }
    public Guid ProductId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public virtual Size Size { get; set; }
    public virtual Product Product { get; set; }
}
