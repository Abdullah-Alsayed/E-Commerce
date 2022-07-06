using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL
{
    public class Applicationdbcontext : IdentityDbContext<User>
    {
        public Applicationdbcontext(DbContextOptions<Applicationdbcontext> options) : base(options) { }
        
        public DbSet<Area> Areas { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }             
        public DbSet<Color> Colors { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Errors> Errors { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Prodact> Prodacts { get; set; }
        public DbSet<ProdactImg> ProdactImgs { get; set; }
        public DbSet<ProdactOrder> ProdactOrders { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            Builder.Entity<Area>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Brand>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

             Builder.Entity<Category>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Errors>()
            .Property(s => s.Date)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Expense>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Favorite>()
            .Property(s => s.Date)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Governorate>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Notification>()
            .Property(s => s.CreationDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Order>()
            .Property(s => s.Createdate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Prodact>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<PromoCode>()
            .Property(s => s.CreateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Review>()
            .Property(s => s.CrateDate)
            .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Slider>()
           .Property(s => s.CreateDate)
           .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Status>()
           .Property(s => s.CreateDate)
           .HasDefaultValueSql("GETDATE()");

            Builder.Entity<SubCategory>()
           .Property(s => s.CreateDate)
           .HasDefaultValueSql("GETDATE()");

            Builder.Entity<Unit>()
           .Property(s => s.CreateDate)
           .HasDefaultValueSql("GETDATE()");

            Builder.Entity<User>()
           .Property(s => s.CreatDate)
           .HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(Builder);

        }
    }
}
