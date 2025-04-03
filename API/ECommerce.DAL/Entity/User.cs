using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Core;
using ECommerce.DAL.Enums;
using ECommerce.DAL.Interface;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Entity
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
            Governorates = new HashSet<Governorate>();
            SubCategories = new HashSet<SubCategory>();
            Reviews = new HashSet<ProductReview>();
            PromoCodes = new HashSet<Voucher>();
            Categories = new HashSet<Category>();
            Favorites = new HashSet<Favorite>();
            Products = new HashSet<Product>();
            Expenses = new HashSet<Expense>();
            Settings = new HashSet<Setting>();
            Statuses = new HashSet<Status>();
            Sliders = new HashSet<Slider>();
            Orders = new HashSet<Order>();
            Brands = new HashSet<Brand>();
            Areas = new HashSet<Area>();
            Units = new HashSet<Unit>();
        }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
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

        public Guid? CreateBy { get; set; }
        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }
        public DateTime? ModifyAt { get; set; }

        public Guid ModifyBy { get; set; }
        public Guid DeletedBy { get; set; }

        [StringLength(50)]
        public string Language { get; set; } = Constants.Languages.Ar;

        [StringLength(255)]
        public string Photo { get; set; } = Constants.DefaultPhotos.User;

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Slider> Sliders { get; set; }
        public virtual ICollection<Governorate> Governorates { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Voucher> PromoCodes { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
        public virtual ICollection<ProductReview> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}
