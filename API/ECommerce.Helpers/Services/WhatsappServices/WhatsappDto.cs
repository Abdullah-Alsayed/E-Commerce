using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.WhatsappServices
{
    public class WhatsappDto
    {
        public string Number { get; set; }
        public string Language { get; set; }
        public string Template { get; set; }
        public List<WhatsAppComponent> components { get; set; }
    }
}
