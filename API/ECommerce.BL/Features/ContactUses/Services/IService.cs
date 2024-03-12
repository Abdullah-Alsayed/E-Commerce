using System.Threading.Tasks;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.ContactUses.Services
{
    public interface IContactUsService
    {
        Task<BaseResponse> CreateAsync(CreateContactUsRequest request);
        Task<BaseResponse> GetAllAsync(GetAllContactUsRequest request);
    }
}
