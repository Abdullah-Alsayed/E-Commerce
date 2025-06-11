using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Histories.Dtos;
using ECommerce.BLL.Features.Histories.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Histories.Services
{
    public interface IHistoryService
    {
        Task<BaseResponse<BaseGridResponse<List<HistoryDto>>>> GetAllAsync(
            GetAllHistoryRequest request
        );
    }
}
