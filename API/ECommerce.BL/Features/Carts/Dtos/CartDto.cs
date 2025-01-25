using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Carts.Dtos
{
    public record CartDto : BaseEntityDto
    {
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
