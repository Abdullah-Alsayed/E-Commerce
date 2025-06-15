using System;

namespace ECommerce.DAL.Entity
{
    public class ProductTag
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public Guid TagID { get; set; }

        public virtual Product Product { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
