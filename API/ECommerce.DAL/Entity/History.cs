using System;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.DAL.Enums;

namespace ECommerce.DAL.Entity
{
    public class History
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string UserID { get; set; }
        public OperationTypeEnum Action { get; set; }
        public EntitiesEnum Entity { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}
