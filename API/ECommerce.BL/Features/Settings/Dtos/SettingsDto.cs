using ECommerce.BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Settings.Dtos
{
    public record SettingDto : BaseEntityDto
    {
        public string Title { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string MainColor { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Whatsapp { get; set; }
        public string TikTok { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BookingMessage { get; set; }
    }
}
