using System.ComponentModel.DataAnnotations;
using ECommerce.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Collections.Requests
{
    public record CreateCollectionRequest
    {
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        public IFormFile FormFile { get; set; }

        public CollectionType Type { get; set; } = CollectionType.Manual;

        public string Rules { get; set; }
    }
}
