using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Tags.Requests
{
    public record CreateTagRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
    }
}
