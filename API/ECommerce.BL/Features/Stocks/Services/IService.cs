// Ignore Spelling: BLL

using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Stocks.Services
{
    public interface IStockService
    {
        Task<BaseResponse> CreateAsync(CreateStockRequest request);
        Task<BaseResponse> FindAsync(FindStockRequest request);
        Task<BaseResponse<BaseGridResponse<List<StockDto>>>> GetAllAsync(
            GetAllStockRequest request
        );
        Task<BaseResponse> ReturnAsync(ReturnStockRequest request);
        Task<BaseResponse> GetSearchEntityAsync();
    }
}
