using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class SliderPhoto : BaseEntity
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Required, StringLength(100)]
        public string PhotoPath { get; set; }
    }
}
