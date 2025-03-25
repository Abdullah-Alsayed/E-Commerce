using System;
using System.ComponentModel.DataAnnotations;
using ECommerce.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class Role : IdentityRole
    {
        public string NameEn { get; set; }
        public string Description { get; set; }

        public RoleTypeEnum RoleType { get; set; } = RoleTypeEnum.User;

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string CreateBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyBy { get; set; }
    }
}
