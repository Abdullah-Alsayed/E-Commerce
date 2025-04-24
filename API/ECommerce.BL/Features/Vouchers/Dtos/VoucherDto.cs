using System;
using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Vouchers.Dtos
{
    public record VoucherDto : BaseEntityDto
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public int Used { get; set; }

        public int? Max { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
