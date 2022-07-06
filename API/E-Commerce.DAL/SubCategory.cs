using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class SubCategory
    {
        public int ID { get; set; }

        [Column(name: "Name-AR"),StringLength(450)]
        public string? NameAR { get; set; }

        [Column(name: "Name-EN"), StringLength(450)]
        public string? NameEN { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public int CategoryID { get; set; }

        [StringLength(255)]
        public string? Img { get; set; }

        [ForeignKey("User"),Required , StringLength(450)]
        public string CreateBy { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }
    }
}
