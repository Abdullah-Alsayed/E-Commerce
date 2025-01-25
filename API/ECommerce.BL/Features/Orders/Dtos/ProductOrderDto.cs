using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Products.Dtos;

namespace ECommerce.BLL.Features.Orders.Dtos
{
    public class ProductOrderDto
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }
    }
}
