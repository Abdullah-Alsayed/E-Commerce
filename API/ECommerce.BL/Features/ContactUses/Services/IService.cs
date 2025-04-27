using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.BLL.Features.ContactUses.Dtos;
using ECommerce.BLL.Features.ContactUses.Requests;
using ECommerce.BLL.Features.ContactUss.Requests;
using ECommerce.BLL.Response;

namespace ECommerce.BLL.Features.ContactUses.Services
{
    public interface IContactUsService
    {
        Task<BaseResponse> CreateAsync(CreateContactUsRequest request);
        Task<BaseResponse> FindAsync(FindContactUsRequest request);
        Task<BaseResponse> DeleteAsync(DeleteContactUsRequest request);

        Task<BaseResponse<BaseGridResponse<List<ContactUsDto>>>> GetAllAsync(
            GetAllContactUsRequest request
        );
    }
}
