using System;
using ECommerce.BLL.DTO;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Features.Users.Dtos
{
    public record UserDto : BaseEntityDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public UserGanderEnum Gander { get; set; }
        public string Language { get; set; } = "ar-EG";
        public double Discount { get; set; }
        public double MaxUseDiscount { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
