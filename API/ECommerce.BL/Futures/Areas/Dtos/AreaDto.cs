using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Futures.Areas.Dtos
{
    public class AreaDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
