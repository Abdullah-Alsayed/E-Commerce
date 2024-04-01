using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }

        public bool IsMaster { get; set; } = false;

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public string CreateBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyBy { get; set; }
    }
}
