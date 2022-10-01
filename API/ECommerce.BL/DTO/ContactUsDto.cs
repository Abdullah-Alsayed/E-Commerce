using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.BLL.DTO
{
    public class ContactUsDto
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(70)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(255)]
        public string Messege { get; set; }
        public DateTime date { get; set; }
    }
}
