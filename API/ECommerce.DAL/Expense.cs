using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;



namespace ECommerce.DAL
{
    public class Expense
    {
        public int ID { get; set; }

        [Required]
        public double Amount { get; set; }

        [StringLength(150),Required]
        public string Reference { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("User"), Required, StringLength(450)]
        public string CreateBy { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public virtual User User { get; set; }
    }
}
