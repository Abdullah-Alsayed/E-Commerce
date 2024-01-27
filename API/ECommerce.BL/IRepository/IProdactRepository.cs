using ECommerce.DAL.Entity;
using System.Threading.Tasks;

namespace ECommerce.BLL.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> AddToChart(int ID);
    }
}
