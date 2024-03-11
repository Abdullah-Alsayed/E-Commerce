using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Units.Requests
{
    public record CreateUnitRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
