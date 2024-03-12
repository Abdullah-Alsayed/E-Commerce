using ECommerce.BLL.Request;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Expenses.Requests
{
    public record UpdateExpenseRequest : BaseRequest
    {
        public double Amount { get; set; }
        public string Reference { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
