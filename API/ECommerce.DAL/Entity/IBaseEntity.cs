using System;
using ECommerce.DAL.Entity;

namespace ECommerce.DAL.Interface
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        Guid? CreateBy { get; set; }
        Guid ModifyBy { get; set; }
        Guid DeletedBy { get; set; }
        DateTime CreateAt { get; set; }
        DateTime? ModifyAt { get; set; }
        DateTime? DeletedAt { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
