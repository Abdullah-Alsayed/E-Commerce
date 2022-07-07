using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
namespace ECommerce.DAL
{
    public class Unit
    {
        public Unit()
        {
            Prodacts = new  HashSet<Prodact>();
        }
        public int ID { get; set; }

        [StringLength(100)  ,Required]
        public string NameAR {get;set;}

        [StringLength(100) , Required]
        public string NameEN { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("User"), Required, StringLength(450)]
        public string CreateBy { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Prodact> Prodacts { get; set; }
    }
}
