using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.DTO;
using ECommerce.Core.Enums;

namespace ECommerce.BLL.Features.Collections.Dtos
{
    public record CollectionDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string NameAR { get; set; }

        [StringLength(100), Required]
        public string NameEN { get; set; }

        public string PhotoPath { get; set; }

        public CollectionType Type { get; set; } = CollectionType.Manual;

        public string Rules { get; set; }
    }
}
