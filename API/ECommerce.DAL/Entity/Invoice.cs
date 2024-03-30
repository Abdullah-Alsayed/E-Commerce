using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Invoice : BaseEntity
    {
        [Required]
        public Guid OrderId { get; set; }
        public bool IsReturn { get; set; } = false;

        public virtual Order Order { get; set; }
    }
}
