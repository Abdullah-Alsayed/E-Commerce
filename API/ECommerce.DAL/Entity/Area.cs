using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Area : BaseEntity
    {
        public Area() => Orders = new HashSet<Order>();

        public Guid GovernorateID { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }

        public virtual Governorate Governorate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
