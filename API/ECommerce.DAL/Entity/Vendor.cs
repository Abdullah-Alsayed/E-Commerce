using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Vendor : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
    }
}
