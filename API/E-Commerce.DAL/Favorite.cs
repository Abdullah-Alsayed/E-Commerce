using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.DAL
{
    public class Favorite
    {
        public int ID { get; set; }

        [ForeignKey("prodact") , Required]
        public int ProdactID { get; set; }

        [ForeignKey("User"), Required, StringLength(450)]
        public string UserID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
        public virtual Prodact prodact { get; set; }

    }
}