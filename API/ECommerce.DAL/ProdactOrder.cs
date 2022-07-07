using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
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
