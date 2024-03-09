using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Vouchers.Requests
{
    public record UpdateVoucherRequest : BaseRequest
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
