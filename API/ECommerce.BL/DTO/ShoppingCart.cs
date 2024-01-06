using System.Collections.Generic;
using System.Linq;

namespace ECommerce.BLL.DTO
{
    public class ShoppingCartDto
    {
        public double SubTotal { get; set; }

        public int Count { get; set; }

        public virtual ICollection<ProductDto> Products { get; set; }

        public double SUBTOTAL() => Products.Sum(s => s.TotalPrice());
    }
}
