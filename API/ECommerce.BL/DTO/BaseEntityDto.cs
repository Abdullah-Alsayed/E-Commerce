using System;

namespace ECommerce.BLL.DTO
{
    public record BaseEntityDto
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public string DeletedBy { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
