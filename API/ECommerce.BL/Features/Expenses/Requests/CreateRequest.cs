using Microsoft.AspNetCore.Http;

namespace ECommerce.BLL.Features.Expenses.Requests
{
    public record CreateExpenseRequest
    {
        public double Amount { get; set; }
        public string Reference { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
