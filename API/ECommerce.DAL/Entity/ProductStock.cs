﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class ProductStock : BaseEntity
    {
        [Required]
        public Guid ProductID { get; set; }

        [Required]
        public Guid VendorID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = new Product();
        public virtual Vendor Vendor { get; set; } = new Vendor();
    }
}
