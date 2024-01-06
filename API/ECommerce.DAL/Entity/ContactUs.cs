using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class ContactUs : BaseEntity
    {
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
        public string Message { get; set; }
    }
}
