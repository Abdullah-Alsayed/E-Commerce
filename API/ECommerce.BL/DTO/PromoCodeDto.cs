using ECommerce.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.DTO
{
    public class PromoCodeDto
    {
        public int ID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Value { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
    }
}
