using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class ProdactImg
    {
        public int ID { get; set; }
        public int ProdactID { get; set; }

        [Required , StringLength(255)]
        public string Img { get; set; }
        public DateTime CrateAt { get; set; }

        public virtual Prodact Prodact { get; set; }
    }
}
