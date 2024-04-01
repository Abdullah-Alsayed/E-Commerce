using System;
using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Users.Requests
{
    public record UpdateUserRequest : BaseRequest
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string Value { get; set; }
    }
}
