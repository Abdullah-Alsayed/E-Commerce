using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Core.Enums;
using ECommerce.DAL.Interface;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public string NameEn { get; set; }
        public string Description { get; set; }

        public RoleTypeEnum RoleType { get; set; } = RoleTypeEnum.User;

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(User))]
        public Guid? CreateBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid DeletedBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public Guid ModifyBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
    }
}
