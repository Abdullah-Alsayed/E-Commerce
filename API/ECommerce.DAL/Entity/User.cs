using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class User : IdentityUser
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
            SliderPhotos = new HashSet<SliderPhoto>();
            Governorates = new HashSet<Governorate>();
            SubCategorys = new HashSet<SubCategory>();
            PromoCodes = new HashSet<Voucher>();
            Categorys = new HashSet<Category>();
            Favorites = new HashSet<Favorite>();
            Products = new HashSet<Product>();
            Expenses = new HashSet<Expense>();
            Settings = new HashSet<Setting>();
            Statuses = new HashSet<Status>();
            Reviews = new HashSet<Review>();
            Brands = new HashSet<Brand>();
            Orders = new HashSet<Order>();
            Areas = new HashSet<Area>();
            Units = new HashSet<Unit>();
        }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public UserGanderEnum Gander { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Range(1, 90)]
        public int Age { get; set; }
        public double Discount { get; set; }
        public double MaxUseDiscount { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public string CreateBy { get; set; }
        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [StringLength(50)]
        public string Language { get; set; } = "ar-EG";
        public virtual ICollection<SliderPhoto> SliderPhotos { get; set; }
        public virtual ICollection<Governorate> Governorates { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<SubCategory> SubCategorys { get; set; }
        public virtual ICollection<Voucher> PromoCodes { get; set; }
        public virtual ICollection<Category> Categorys { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}
