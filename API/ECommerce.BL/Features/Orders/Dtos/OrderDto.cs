using System;
using System.Collections.Generic;
using ECommerce.BLL.DTO;
using ECommerce.BLL.Features.Areas.Dtos;
using ECommerce.BLL.Features.Governorates.Dtos;
using ECommerce.BLL.Features.Statuses.Dtos;
using ECommerce.BLL.Features.Users.Dtos;
using ECommerce.DAL.Entity;

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
        public double Total { get; set; }
        public bool IsAccept { get; set; }

        public AreaDto Area { get; set; }
        public StatusDto Status { get; set; }
        public GovernorateDto Governorate { get; set; }
        public UserDto User { get; set; }

        public List<ProductOrderDto> ProductOrders { get; set; }
    }
}
