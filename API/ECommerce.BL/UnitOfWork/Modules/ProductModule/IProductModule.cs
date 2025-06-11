using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ProductModule
{
    public interface IProductModule
    {
        IProductRepository Product { get; }
        IBaseRepository<Size> Size { get; }
        IBaseRepository<Tag> Tag { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Brand> Brand { get; }
        IBaseRepository<Category> Category { get; }
        IBaseRepository<SubCategory> SubCategory { get; }
        IBaseRepository<Unit> Unit { get; }
        IBaseRepository<Booking> Booking { get; }
        IBaseRepository<ProductReview> Review { get; }
    }
}
