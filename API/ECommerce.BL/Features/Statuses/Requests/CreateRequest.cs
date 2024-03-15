using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Statuses.Requests
{
    public record CreateStatusRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public bool IsCompleted { get; set; }
    }
}
