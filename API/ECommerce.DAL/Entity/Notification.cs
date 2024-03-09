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
        public string MessageEN { get; set; }
        public string Icon { get; set; }
        public OperationTypeEnum operationTypeEnum { get; set; }
        public string EntityName { get; set; }
        public string CreateName { get; set; }
    }
}
