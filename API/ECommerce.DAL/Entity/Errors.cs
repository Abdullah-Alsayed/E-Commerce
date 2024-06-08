using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.DAL.Enums;

namespace ECommerce.DAL.Entity
{
    public class ErrorLog
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Endpoint { get; set; }

        public EntitiesEnum Entity { get; set; }
        public OperationTypeEnum Operation { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
