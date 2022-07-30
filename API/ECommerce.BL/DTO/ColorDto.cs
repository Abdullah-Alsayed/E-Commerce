using ECommerce.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class ColorDto
    {
        public int ID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }
      //  public virtual ICollection<Prodact> prodacts { get; set; }

    }
}
