using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Features.Users.Requests
{
    public class ConfirmEmailUserRequest
    {
        public string ID { get; set; }
        public string Token { get; set; }
    }
}
