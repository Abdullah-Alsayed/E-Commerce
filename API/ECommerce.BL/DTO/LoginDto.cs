using ECommerce.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class LoginDto
    {
        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Rememberme { get; set; }
    }
}
