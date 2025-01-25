using ECommerce.BLL.DTO;

namespace ECommerce.BLL.Features.Expenses.Dtos
{
    public record ExpenseDto : BaseEntityDto
    {
        public double Amount { get; set; }
        public string Reference { get; set; }
        public string PhotoPath { get; set; }
    }
}
