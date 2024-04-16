using System;

namespace ECommerce.DAL.Entity
{
    public class Booking : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid ColorID { get; set; }
        public Guid SizeID { get; set; }

        public bool IsNotified { get; set; } = false;
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
        public virtual Color Color { get; set; }
    }
}
