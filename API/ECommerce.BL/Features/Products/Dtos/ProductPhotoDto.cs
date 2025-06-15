using System;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Products.Dtos
{
    public class ProductPhotoDto
    {
        public IFormFile Photo { get; set; }
        public Guid ColorId { get; set; }
    }
}
