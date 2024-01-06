using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Section : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Order { get; set; }
    }
}
