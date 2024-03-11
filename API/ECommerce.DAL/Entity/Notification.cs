using System.ComponentModel.DataAnnotations;
using ECommerce.DAL.Enums;

namespace ECommerce.DAL.Entity
{
    public class Notification : BaseEntity
    {
        [StringLength(100), Required]
        public string Title { get; set; }

        [StringLength(100), Required]
        public string Subject { get; set; }

        [StringLength(255), Required]
        public string MessageAR { get; set; }

        [StringLength(255), Required]
        public string MessageEN { get; set; }
        public OperationTypeEnum OperationType { get; set; }
        public EntitiesEnum Entity { get; set; }
        public string CreateName { get; set; }
    }
}
