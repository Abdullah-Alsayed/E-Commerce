using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Invoices.Dtos;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.Invoices.Services
{
    public interface IInvoiceService
    {
        Task<BaseResponse> CreateAsync(CreateInvoiceRequest request);
        Task<BaseResponse> DeleteAsync(DeleteInvoiceRequest request);
        Task<BaseResponse> FindAsync(FindInvoiceRequest request);
        Task<BaseResponse<BaseGridResponse<List<InvoiceDto>>>> GetAllAsync(
            GetAllInvoiceRequest request
        );
        Task<BaseResponse> GetSearchEntityAsync();
        Task<BaseResponse> ReturnAsync(ReturnInvoiceRequest request);
    }
}
