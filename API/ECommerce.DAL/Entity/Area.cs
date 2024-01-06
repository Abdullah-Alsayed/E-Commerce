using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL.Entity
{
    public class Area : BaseEntity
    {
        public Area() => Orders = new HashSet<Order>();

        [Required]
        public Guid GovernorateID { get; set; }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        public virtual Governorate Governorate { get; set; } = new Governorate();
        public virtual ICollection<Order> Orders { get; set; }
    }
}
