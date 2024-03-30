using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Orders.Dtos
{
    public record OrderDto : BaseEntityDto
    {
        public Guid AreaID { get; set; }
        public Guid StatusID { get; set; }
        public Guid GovernorateID { get; set; }
        public Guid? VoucherID { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsOffLine { get; set; }
        public int Tax { get; set; }
        public int Count { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public bool IsAccept { get; set; }
    }
}
