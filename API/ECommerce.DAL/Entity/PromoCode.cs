using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECommerce.DAL.Entity
{
    public class PromoCode : BaseEntity
    {
        public PromoCode() => Orders = new HashSet<Order>();

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Value { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
