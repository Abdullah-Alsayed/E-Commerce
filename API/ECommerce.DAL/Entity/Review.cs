using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class ProductReview : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required, StringLength(255)]
        public string Review { get; set; }

        [Required, Range(1, 5)]
        public int Rate { get; set; }

        public virtual Product Product { get; set; }
    }
}
