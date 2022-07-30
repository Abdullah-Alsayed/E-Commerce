using ECommerce.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class RegisterDto
    {
        [Required, StringLength(100)]
        public string FitsrName { get; set; }

        [Required, StringLength(100)]
        public string lastName { get; set; }
        [Required]
        public DateTime CreatDate { get; set; }

        [Required, StringLength(100)]

        public string Gander { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Range(1, 90)]
        public int Age { get; set; }

        [StringLength(50)]
        public string? language { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password) , StringLength(100)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ComparePassword { get; set; }
        public bool Rememberme { get; set; }

        [Required, RegularExpression(@"^([0-9]{11})$", ErrorMessage = "The PhoneNumber field is not a valid")]
        public string PhoneNumber { get; set; }
    }
}
