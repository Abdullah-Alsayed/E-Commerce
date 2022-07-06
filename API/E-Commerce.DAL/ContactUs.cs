using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DAL
{
    public class ContactUs
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9]{1,11}$")]
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
