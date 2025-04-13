using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Tag : BaseEntity
    {
        [Required, StringLength(100)]
        public string NameAR { get; set; }

        [Required, StringLength(100)]
        public string NameEN { get; set; }
    }
}
