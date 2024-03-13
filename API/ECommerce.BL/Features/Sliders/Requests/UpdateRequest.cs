using System;
using ECommerce.BLL.Request;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Sliders.Requests
{
    public record UpdateSliderRequest : BaseRequest
    {
        public string TitleAR { get; set; }
        public string TitleEN { get; set; }
        public string Description { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
