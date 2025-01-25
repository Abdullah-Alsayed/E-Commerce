using System.Threading.Tasks;
using ECommerce.BLL.Features.Expenses.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Expenses.Services
{
    public interface IExpenseService
    {
        Task<BaseResponse> CreateAsync(CreateExpenseRequest request);
        Task<BaseResponse> DeleteAsync(DeleteExpenseRequest request);
        Task<BaseResponse> FindAsync(FindExpenseRequest request);
        Task<BaseResponse> GetAllAsync(GetAllExpenseRequest request);
        Task<BaseResponse> UpdateAsync(UpdateExpenseRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
