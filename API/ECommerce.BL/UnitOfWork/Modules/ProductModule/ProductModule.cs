using System;
using ECommerce.BLL.Repository;
using ECommerce.BLL.Repository.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.UnitOfWork.Modules.ProductModule
{
    public class ProductModule : IProductModule
    {
        private readonly ApplicationDbContext _context;

        private readonly Lazy<IProductRepository> _product;
        private readonly Lazy<IBaseRepository<Size>> _size;
        private readonly Lazy<IBaseRepository<Color>> _color;
        private readonly Lazy<IBaseRepository<Brand>> _brand;
        private readonly Lazy<IBaseRepository<Unit>> _unit;
        private readonly Lazy<IBaseRepository<Category>> _category;
        private readonly Lazy<IBaseRepository<SubCategory>> _subCategory;
        private readonly Lazy<IBaseRepository<Booking>> _booking;
        private readonly Lazy<IBaseRepository<ProductReview>> _review;
        private readonly Lazy<IBaseRepository<Tag>> _tag;

        public ProductModule(ApplicationDbContext context)
        {
            _context = context;

            _product = new Lazy<IProductRepository>(() => new ProductRepository(_context));
            _size = new Lazy<IBaseRepository<Size>>(() => new BaseRepository<Size>(_context));
            _color = new Lazy<IBaseRepository<Color>>(() => new BaseRepository<Color>(_context));
            _brand = new Lazy<IBaseRepository<Brand>>(() => new BaseRepository<Brand>(_context));
            _unit = new Lazy<IBaseRepository<Unit>>(() => new BaseRepository<Unit>(_context));
            _category = new Lazy<IBaseRepository<Category>>(
                () => new BaseRepository<Category>(_context)
            );
            _subCategory = new Lazy<IBaseRepository<SubCategory>>(
                () => new BaseRepository<SubCategory>(_context)
            );
            _booking = new Lazy<IBaseRepository<Booking>>(
                () => new BaseRepository<Booking>(_context)
            );
            _review = new Lazy<IBaseRepository<ProductReview>>(
                () => new BaseRepository<ProductReview>(_context)
            );
            _tag = new Lazy<IBaseRepository<Tag>>(() => new BaseRepository<Tag>(_context));
        }

        public IProductRepository Product => _product.Value;
        public IBaseRepository<Size> Size => _size.Value;
        public IBaseRepository<Color> Color => _color.Value;
        public IBaseRepository<Brand> Brand => _brand.Value;
        public IBaseRepository<Unit> Unit => _unit.Value;
        public IBaseRepository<Category> Category => _category.Value;
        public IBaseRepository<SubCategory> SubCategory => _subCategory.Value;
        public IBaseRepository<Booking> Booking => _booking.Value;
        public IBaseRepository<ProductReview> Review => _review.Value;
        public IBaseRepository<Tag> Tag => _tag.Value;
    }
}
