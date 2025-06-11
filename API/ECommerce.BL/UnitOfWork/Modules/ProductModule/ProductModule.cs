using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ProductModule
{
    public class ProductModule : IProductModule
    {
        private readonly ApplicationDbContext _context;

        public ProductModule(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Product => _product ??= new ProductRepository(_context);
        private IProductRepository _product;

        public IBaseRepository<Size> Size => _size ??= new BaseRepository<Size>(_context);
        private IBaseRepository<Size> _size;

        public IBaseRepository<Color> Color => _color ??= new BaseRepository<Color>(_context);
        private IBaseRepository<Color> _color;

        public IBaseRepository<Brand> Brand => _brand ??= new BaseRepository<Brand>(_context);
        private IBaseRepository<Brand> _brand;

        private IBaseRepository<Unit> _unit;
        public IBaseRepository<Unit> Unit => _unit ??= new BaseRepository<Unit>(_context);

        public IBaseRepository<Category> Category =>
            _category ??= new BaseRepository<Category>(_context);
        private IBaseRepository<Category> _category;

        public IBaseRepository<SubCategory> SubCategory =>
            _subCategory ??= new BaseRepository<SubCategory>(_context);
        private IBaseRepository<SubCategory> _subCategory;

        public IBaseRepository<Booking> Booking =>
            _booking ??= new BaseRepository<Booking>(_context);
        private IBaseRepository<Booking> _booking;

        public IBaseRepository<ProductReview> Review =>
            _review ??= new BaseRepository<ProductReview>(_context);
        private IBaseRepository<ProductReview> _review;

        public IBaseRepository<Tag> Tag => _tag ??= new BaseRepository<Tag>(_context);
        private IBaseRepository<Tag> _tag;
    }
}
