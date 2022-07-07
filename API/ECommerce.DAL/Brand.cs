using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.DAL
{
    public class Brand
    {
        public Brand()
        {
            Prodacts = new HashSet<Prodact>();
        }

        public int ID { get; set; }

        [StringLength(100) , Required]
        public string NameAR {get;set;}

        [StringLength(100),Required]
        public string NameEN { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [StringLength(255)]
        public string? ImgID { get; set; }

        [ForeignKey("User"), StringLength(450),Required]
        public string CreateBy { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Prodact> Prodacts { get; set; }

    }
}
