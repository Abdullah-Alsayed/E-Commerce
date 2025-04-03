using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.DAL.Interface;

namespace ECommerce.DAL.Entity
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User)), StringLength(450), Required]
        public Guid? CreateBy { get; set; }

        [StringLength(450)]
        public Guid ModifyBy { get; set; }

        [StringLength(450)]
        public Guid DeletedBy { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
    }
}
