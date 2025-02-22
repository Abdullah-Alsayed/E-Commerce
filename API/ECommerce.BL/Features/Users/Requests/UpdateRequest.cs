using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Users.Requests
{
    public record UpdateUserRequest : BaseRequest
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public string RoleId { get; set; }

        [Required]
        public UserGanderEnum Gander { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Range(1, 90)]
        public int Age { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile ProfilePicture { get; set; }
    }
}
