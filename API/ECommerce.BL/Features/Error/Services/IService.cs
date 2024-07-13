using System.Threading.Tasks;
using ECommerce.BLL.Response;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Features.Errors.Services
{
    public interface IErrorService
    {
        Task<BaseResponse> GetAllAsync(EntitiesEnum Entity);
    }
}
