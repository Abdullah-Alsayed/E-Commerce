using System.ComponentModel;

namespace ECommerce.BLL.Features.Vouchers.Requests
{
    public record CreateVoucherRequest
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
