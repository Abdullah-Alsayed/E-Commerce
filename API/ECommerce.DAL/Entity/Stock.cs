using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Stock : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required]
        public Guid VendorID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
