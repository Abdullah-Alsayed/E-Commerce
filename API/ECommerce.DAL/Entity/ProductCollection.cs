using System;

namespace ECommerce.DAL.Entity
{
    public class ProductCollection
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public Guid CollectionID { get; set; }

        public virtual Product Product { get; set; }
        public virtual Collection Collection { get; set; }
    }
}
