using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.ContactUses.Dtos
{
    public record ContactUsDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
