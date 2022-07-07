using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class Errors
    {
        public int ID { get; set; }

        [MaxLength(100)]
        public int Code { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Messege { get; set; }

        public DateTime? Date { get; set; }
    }
}
