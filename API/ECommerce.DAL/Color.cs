using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class Color
    {
        public Color()
        {
            prodacts = new HashSet<Prodact>();
        }
        public int ID { get; set; }

        [Required , StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }

        public virtual ICollection<Prodact> prodacts { get; set; }

    }
}