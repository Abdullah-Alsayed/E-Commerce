using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity;

public class Size : BaseEntity
{
    public Size() => ProductSizes = new HashSet<ProductSize>();

    [Required, StringLength(100)]
    public string NameAR { get; set; }
    public string NameEN { get; set; }

    public virtual ICollection<ProductSize> ProductSizes { get; set; }
}
