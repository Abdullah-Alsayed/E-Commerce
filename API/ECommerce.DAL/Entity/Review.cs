using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class Review : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required, StringLength(255)]
        public string Reviews { get; set; }

        [Required, Range(1, 5)]
        public int Rate { get; set; }

        public virtual Product Product { get; set; } = new Product();
    }
}
