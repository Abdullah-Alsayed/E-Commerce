using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Status : BaseEntity
    {
        public Status() => Orders = new HashSet<Order>();

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [Range(1, int.MaxValue), Required]
        public int Order { get; set; }
        public bool IsCompleted { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
