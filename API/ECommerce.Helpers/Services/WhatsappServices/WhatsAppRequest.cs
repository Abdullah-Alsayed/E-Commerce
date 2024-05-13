using System.Collections.Generic;

namespace ECommerce.Core.Services.WhatsappServices
{
    public class WhatsAppRequest
    {
        public string messaging_product { get; set; } = "whatsapp";
        public string recipient_type { get; set; } = "individual";
        public string to { get; set; }
        public string type { get; set; } = "template";
        public WhatsAppTemplate template { get; set; }
    }

    public class WhatsAppTemplate
    {
        public string name { get; set; }
        public WhatsAppLanguage language { get; set; }
        public List<WhatsAppComponent> components { get; set; }
    }

    public class WhatsAppLanguage
    {
        public string code { get; set; }
    }

    public class WhatsAppComponent
    {
        public string type { get; set; }
        public List<object> parameters { get; set; }
    }
}
