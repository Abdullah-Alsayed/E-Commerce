using System;
using ECommerce.BLL.Interface;
using ECommerce.DAL.Interface;

namespace ECommerce.BLL.DTO
{
    public record BaseEntityDto : IBaseEntityDto
    {
        public Guid Id { get; set; }
        public Guid CreateBy { get; set; }
        public Guid ModifyBy { get; set; }
        public Guid DeletedBy { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
