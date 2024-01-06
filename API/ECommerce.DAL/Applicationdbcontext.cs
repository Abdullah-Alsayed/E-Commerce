using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class Applicationdbcontext : IdentityDbContext<User>
    {
        public Applicationdbcontext(DbContextOptions<Applicationdbcontext> options)
            : base(options) { }

        //public DbSet<Area> Areas { get; set; }
        //public DbSet<Brand> Brands { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Color> Colors { get; set; }
        //public DbSet<ContactUs> ContactUs { get; set; }
        //public DbSet<ErrorLogs> ErrorLogs { get; set; }
        //public DbSet<Expense> Expenses { get; set; }
        //public DbSet<Favorite> Favorites { get; set; }
        //public DbSet<Governorate> Governorates { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<ProductPhoto> productPhotos { get; set; }
        //public DbSet<ProductOrder> ProductOrders { get; set; }
        //public DbSet<PromoCode> PromoCodes { get; set; }
        //public DbSet<Review> Reviews { get; set; }
        //public DbSet<Setting> Settings { get; set; }
        //public DbSet<SliderPhotos> SliderPhotos { get; set; }
        //public DbSet<Status> Statuses { get; set; }
        //public DbSet<SubCategory> SubCategories { get; set; }
        //public DbSet<Unit> Units { get; set; }
        //public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);
        }
    }
}
