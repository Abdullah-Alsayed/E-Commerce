using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Features.Area.Requests
{
    public class CreateAreaRequest
    {
        public Guid GovernorateID { get; set; }

        public string NameAR { get; set; }

        public string NameEN { get; set; }
    }
}
