using System;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.SubCategories.Requests
{
    public record CreateSubCategoryRequest
    {
        public Guid CategoryID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
