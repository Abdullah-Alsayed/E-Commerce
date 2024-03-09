using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Colors.Requests
{
    public record CreateColorRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Value { get; set; }
    }
}
