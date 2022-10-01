using ECommerce.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.DTO
{
    public class ShoppingCart
    {
        public double SubTotal { get; set; }

        public int Count { get; set; }

        public virtual ICollection<ProdactDto> Prodact { get; set; }

        public double SUBTOTAL()
        {
            return Prodact.Sum(s => s.TotalPrice());
        }
    }
}
