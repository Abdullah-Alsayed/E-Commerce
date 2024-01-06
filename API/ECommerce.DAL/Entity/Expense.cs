using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Expense : BaseEntity
    {
        [Required, Range(1, double.MaxValue)]
        public double Amount { get; set; }

        [StringLength(150), Required]
        public string Reference { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }
    }
}
