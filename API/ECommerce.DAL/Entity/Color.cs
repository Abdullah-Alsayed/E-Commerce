using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Color : BaseEntity
    {
        public Color() => ProductColors = new HashSet<ProductColor>();

        [Required, StringLength(100)]
        public string NameAR { get; set; }
        public string NameEN { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }

        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
