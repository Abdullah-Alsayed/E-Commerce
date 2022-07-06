using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace E_Commerce.DAL
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

        public virtual User User { get; set; }
    }
}
