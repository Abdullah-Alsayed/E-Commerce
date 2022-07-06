using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class ProdactOrder
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProdactID { get; set; }
        public DateTime Date { get; set; }
        public virtual Order Order { get; set; }
        public virtual Prodact Prodact { get; set; }


    }
}
