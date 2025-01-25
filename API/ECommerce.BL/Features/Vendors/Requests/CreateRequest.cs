using System.ComponentModel.DataAnnotations;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Vendors.Requests
{
    public record CreateVendorRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
