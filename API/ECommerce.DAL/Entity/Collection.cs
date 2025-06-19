using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECommerce.Core.Enums;

namespace ECommerce.DAL.Entity
{
    public class Collection : BaseEntity
    {
        public Collection() => ProductCollections = new HashSet<ProductCollection>();

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        public string PhotoPath { get; set; }

        public CollectionType Type { get; set; } = CollectionType.Manual;

        public string Rules { get; set; }

        public virtual ICollection<ProductCollection> ProductCollections { get; set; }
    }
}
