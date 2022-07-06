using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL
{
    public class Applicationdbcontext:IdentityDbContext<IdentityUser>
    {
        public Applicationdbcontext(DbContextOptions<Applicationdbcontext> options) : base(options) { }

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


        }
    }
}
