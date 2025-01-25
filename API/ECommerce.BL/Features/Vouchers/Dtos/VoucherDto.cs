using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Vouchers.Dtos
{
    public record VoucherDto : BaseEntityDto
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
