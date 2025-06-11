using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Vendors.Requests
{
    public record UpdateVendorRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
