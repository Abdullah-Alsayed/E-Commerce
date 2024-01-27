using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository;

public interface IGovernorateRepository : IBaseRepository<Governorate>
{
    Task<BaseResponse> FindGovernorate(FindGovernorateRequest request);
}
