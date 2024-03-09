using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.DAL.Enums;

namespace ECommerce.DAL.Entity
{
    public class ErrorLog
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required, Range(1, int.MaxValue)]
        [StringLength(150)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Message { get; set; }

        public EntitiesEnum Entity { get; set; }
        public OperationTypeEnum Operation { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
