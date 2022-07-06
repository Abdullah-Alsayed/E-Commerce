using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Commerce.DAL
{
    public class PromoCode
    {
        public int ID { get; set; }

        [Required , StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Value { get; set; }

        [ForeignKey("User"),StringLength(450), Required]
        public string CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public virtual User User { get; set; }
    }
}
