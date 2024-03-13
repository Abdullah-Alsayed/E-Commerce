using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Slider : BaseEntity
    {
        [Required, StringLength(100)]
        public string TitleAR { get; set; }

        [Required, StringLength(100)]
        public string TitleEN { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        public string PhotoPath { get; set; }
    }
}
