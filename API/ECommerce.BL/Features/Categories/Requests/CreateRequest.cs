using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Categories.Requests
{
    public record CreateCategoryRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
