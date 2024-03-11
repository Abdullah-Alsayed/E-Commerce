using System;
using ECommerce.BLL.Request;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.SubCategories.Requests
{
    public record UpdateSubCategoryRequest : BaseRequest
    {
        public Guid CategoryID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
