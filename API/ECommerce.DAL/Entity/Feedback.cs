using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Feedback : BaseEntity
    {
        [Required]
        public string Comment { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Rating { get; set; }
    }
}
