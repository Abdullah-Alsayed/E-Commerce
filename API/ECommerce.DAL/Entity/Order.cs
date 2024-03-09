using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Order : BaseEntity
    {
        public Order() => ProductOrders = new HashSet<ProductOrder>();

        [Required]
        public Guid AreaID { get; set; }

        [Required]
        public Guid StatusID { get; set; }

        [Required]
        public Guid GovernorateID { get; set; }

        public Guid PromoCodeID { get; set; }

        [Required, StringLength(255)]
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsOffLine { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Tax { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Count { get; set; }

        [Range(1, double.MaxValue)]
        public double Discount { get; set; }

        [Required, Range(1, double.MaxValue)]
        public double SubTotal { get; set; }

        public virtual Area Area { get; set; } = new Area();
        public virtual Status Status { get; set; } = new Status();
        public virtual Governorate Governorate { get; set; } = new Governorate();
        public virtual Voucher PromoCode { get; set; } = new Voucher();
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
