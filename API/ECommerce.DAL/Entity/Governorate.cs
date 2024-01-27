using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Governorate : BaseEntity
    {
        public Governorate()
        {
            Areas = new HashSet<Area>();
            Orders = new HashSet<Order>();
        }

        public Guid ID { get; set; }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Tax { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
