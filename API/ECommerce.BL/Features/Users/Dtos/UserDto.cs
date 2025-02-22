using System;
using System.Collections.Generic;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Roles.Dtos;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Identity;

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
        public string Photo { get; set; }
        public int Age { get; set; }
        public UserGanderEnum Gander { get; set; }
        public string Language { get; set; } = "ar-EG";
        public double Discount { get; set; }
        public double MaxUseDiscount { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RoleId { get; set; }

        public RoleDto Role { get; set; }
    }
}
