using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Vendors.Dtos
{
    public record VendorDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
