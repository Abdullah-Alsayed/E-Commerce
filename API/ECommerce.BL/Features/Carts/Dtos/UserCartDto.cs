using ECommerce.BLL.Features.Products.Dtos;

namespace ECommerce.BLL.Features.Carts.Dtos
{
    public record UserCartDto : CartDto
    {
        public ProductDto product { get; set; }
    }
}
