using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class Category
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
            Prodacts      = new HashSet<Prodact>();
        }

    public int ID { get; set; }

    [Column(name: "Name-AR"),StringLength(100),Required]
    public string NameAR {get;set;}

    [Column(name: "Name-EN"), StringLength(100), Required]
    public string NameEN { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

    [StringLength(255)]
    public string? ImgID { get; set; }

    [ForeignKey("User"),Required,StringLength(450)]
    public string CreateBy{ get; set; }

    [StringLength(450)]
    public string? ModifyBy { get; set; }
    public DateTime? ModifyAt { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; }
    public virtual ICollection<Prodact> Prodacts { get; set; }

    public virtual User User { get; set; }

    }
}
