using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class StatusDto
    {
        public int ID { get; set; }
        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }


        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }


    }
}
