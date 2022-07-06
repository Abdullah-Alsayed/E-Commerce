using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DAL
{
    public class color
    {
        public color()
        {
            prodacts = new HashSet<Prodact>();
        }
        public int ID { get; set; }

        [Required , StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }

        public virtual ICollection<Prodact> prodacts { get; set; }

    }
}