using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.DAL
{
    public class Unit
    {
        public Unit()
        {
            Prodacts = new  HashSet<Prodact>();
        }
        public int ID { get; set; }

        [Column(name: "Name-AR") , StringLength(100)]
        public string? NameAR {get;set;}

        [Column(name: "Name-EN"), StringLength(100)]
        public string? NameEN { get; set; }

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
