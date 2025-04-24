using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Voucher : BaseEntity
    {
        public Voucher() => Orders = new HashSet<Order>();

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Value { get; set; }

        public int? Max { get; set; }

        public int Used { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
