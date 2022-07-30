using ECommerce.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.BLL.DTO
{
    public class BrandDto
    {

        [StringLength(100), Required]
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        [StringLength(450)]
        public string? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }

        public IFormFile? File { get; set; }
        //public virtual User User { get; set; }
        //public virtual ICollection<Prodact> Prodacts { get; set; }
    }
}
