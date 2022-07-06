using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DAL
{
    public class ProdactImg
    {
        public int ID { get; set; }
        public int ProdactID { get; set; }

        [Required , StringLength(255)]
        public string Img { get; set; }
        public int CrateDate { get; set; }

        public virtual Prodact Prodact { get; set; }
    }
}
