using System;
using ECommerce.BLL.Request;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Brands.Requests
{
    public record UpdateBrandRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
