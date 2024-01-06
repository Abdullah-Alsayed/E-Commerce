using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Vendor : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(11)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Address { get; set; }
    }
}
