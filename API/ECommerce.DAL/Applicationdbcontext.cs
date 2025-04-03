using System;
using System.Linq;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<ProductReview> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<RoleClaims> RoleClaims { get; set; }
        public DbSet<TokenExpired> TokenExpired { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<TEntity> AddDbSet<TEntity>()
            where TEntity : class
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (
                var relationship in builder
                    .Model.GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys())
            )
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
            }

            //builder.Entity<User>(b =>
            //{
            //    // Each User can have many entries in the UserRole join table
            //    b.HasMany(e => e.UserRoles)
            //        .WithOne(e => e.User)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();
            //});
            //builder.Entity<Role>(b =>
            //{
            //    // Each Role can have many entries in the UserRole join table
            //    b.HasMany(e => e.UserRoles)
            //        .WithOne(e => e.Role)
            //        .HasForeignKey(ur => ur.RoleId)
            //        .IsRequired();
            //});

            //builder.Entity<IdentityUserRole<string>>(b =>
            //{
            //    b.ToTable("UserRole");
            //});

            //builder.Entity<UserRole>(userRole =>
            //{
            //    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            //    userRole
            //        .HasOne(ur => ur.Role)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.RoleId);

            //    userRole
            //        .HasOne(ur => ur.User)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.UserId);
            //});
            base.OnModelCreating(builder);
        }
    }
}
