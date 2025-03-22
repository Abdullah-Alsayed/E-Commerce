using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Users.Requests
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoleId { get; set; }

        public UserGanderEnum Gander { get; set; }

        public string Address { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ComparePassword { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile ProfilePicture { get; set; }
    }
}
