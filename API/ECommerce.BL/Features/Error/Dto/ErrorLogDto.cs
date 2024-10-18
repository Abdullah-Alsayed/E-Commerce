using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Features.Error.Dto
{
    public class ErrorLogDto
    {
        public Guid ID { get; set; }

        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Endpoint { get; set; }

        public string Entity { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
