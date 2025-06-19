using System;

namespace ECommerce.DAL.Entity
{
    public class ProductPhoto
    {
        public Guid ID { get; set; }
        public string Photo { get; set; }
        public Guid ProductID { get; set; }
        public Guid ColorID { get; set; }

        public virtual Product Product { get; set; }
        public virtual Color Color { get; set; }
    }
}
