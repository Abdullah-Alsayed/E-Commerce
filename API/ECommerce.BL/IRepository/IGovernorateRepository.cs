using System.Threading.Tasks;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository;

public interface IGovernorateRepository : IBaseRepository<Governorate>
{
    Task<BaseResponse> FindGovernorate(FindGovernorateRequest request);
}
