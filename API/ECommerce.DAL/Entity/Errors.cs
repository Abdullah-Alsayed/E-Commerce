using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.DAL.Entity
{
    public class ErrorLog
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required, Range(1, int.MaxValue)]
        public int Code { get; set; }

        [StringLength(150)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
