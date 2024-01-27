using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Futures.Governorates.Dtos
{
    public class GovernorateDto : BaseEntityDto
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public int Tax { get; set; }
    }
}
