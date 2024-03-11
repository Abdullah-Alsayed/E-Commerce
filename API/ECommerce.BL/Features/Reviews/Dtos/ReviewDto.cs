using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Reviews.Dtos
{
    public record ReviewDto : BaseEntityDto
    {
        public Guid ProductID { get; set; }
        public string Review { get; set; }
        public int Rate { get; set; }
    }
}
