using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.DAL
{
    public class Review
    {
        public int ID { get; set; }

        [ForeignKey("Prodact"),Required]
        public int ProdactID { get; set; }

        [Required , StringLength(255)]
        public string review { get; set; }

        [Required , Range(1,10)]
        public int Rate { get; set; }


        [ForeignKey("User") , StringLength(450) , Required]
        public string UserID { get; set; }

        [Required]
        public DateTime CrateDate { get; set; }

        public virtual Prodact Prodact { get; set; }
        public virtual User User { get; set; }
    }


}