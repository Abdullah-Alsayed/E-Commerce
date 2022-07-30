using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.BLL.DTO
{
    public class AreaDto
    {

        public int ID { get; set; }

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [Required]
        public int GovernorateID { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }

        public DateTime? ModifyAt { get; set; }
        public bool IsActive { get; set; }
    }
}