using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class Favorite : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; } = new Product();
    }
}
