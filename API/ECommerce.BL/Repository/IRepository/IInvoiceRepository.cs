using System.Threading.Tasks;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.Repository.IRepository
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        Task<Invoice> GetInvoiceProductsAsync(ReturnInvoiceRequest request);
    }
}
