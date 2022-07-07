using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class Area
    {
        public Area()
        {
            Orders = new HashSet<Order>();   
        }

        public int ID { get; set; }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [Required]
        public int GovernorateID { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("User"),StringLength(450),Required]
        public string CreateBy { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }

        public DateTime? ModifyAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual Governorate Governorate { get; set; }
        public virtual User User { get; set; }
    }
}