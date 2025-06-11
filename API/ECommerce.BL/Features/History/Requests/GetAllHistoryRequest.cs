using ECommerce.BLL.Request;

namespace ECommerce.BLL.Features.Histories.Requests
{
    public record GetAllHistoryRequest : BaseGridRequest
    {
        public string EntityName { get; set; }
        public string EntityId { get; set; }
    }
}
