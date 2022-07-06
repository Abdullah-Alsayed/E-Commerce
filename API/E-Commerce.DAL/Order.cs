using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DAL
{
    public class Order
    {
        public Order()
        {
            ProdactOrders = new HashSet<ProdactOrder>();
        }
        public int ID { get; set; }

        [ForeignKey("User"),StringLength(450),Required]
        public string UserID { get; set; }

        [Required]
        public int AreaID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int GovernorateID { get; set; }

        public DateTime DeliveryDate { get; set; }
        public bool? ISoffline { get; set; }

        [Required,StringLength(255)]
        public string Address { get; set; }

        [Required]
        public double SubTotal { get; set; }

        [Required]
        public int Tax { get; set; }
        public double Discount { get; set; }

        [Required]
        public int Count { get; set; }
        public bool IsConfirmed { get; set; }

        [ForeignKey("User"),StringLength(450),Required]
        public string CreateBy { get; set; }

        [Required]
        public DateTime Createdate { get; set; }
         
        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public virtual User User { get; set; }
        public virtual Area Area { get; set; }
        public virtual Status Status { get; set; }
        public virtual Governorate Governorate { get; set; }
        public virtual ICollection<ProdactOrder> ProdactOrders { get; set; }
    }
}
