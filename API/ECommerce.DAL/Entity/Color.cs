using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECommerce.DAL.Entity
{
    public class Color : BaseEntity
    {
        public Color() => ProductColors = new HashSet<ProductColor>();

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }

        public virtual ICollection<ProductColor> ProductColors { get; set; }
    }
}
