using System;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Features.Histories.Dtos
{
    public class HistoryDto
    {
        public Guid Id { get; set; }
        public OperationTypeEnum Action { get; set; }
        public EntitiesEnum Entity { get; set; }
        public string UserName { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
