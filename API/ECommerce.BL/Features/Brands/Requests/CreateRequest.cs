using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Brands.Requests
{
    public record CreateBrandRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
