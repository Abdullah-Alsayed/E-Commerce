using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class User : IdentityUser
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
            Favorites     = new HashSet<Favorite>();
            Prodacts      = new HashSet<Prodact>();
            Orders        = new HashSet<Order>();
            Areas         = new HashSet<Area>();
            Governorates  = new HashSet<Governorate>();
            Categorys     = new HashSet<Category>();
            Reviews       = new HashSet<Review>();
            Sliders       = new HashSet<Slider>();
            Statuses      = new HashSet<Status>();
            PromoCodes    = new HashSet<PromoCode>();
            Units         = new HashSet<Unit>();
            Brands        = new HashSet<Brand>();
            Expenses      = new HashSet<Expense>();
            Settings      = new HashSet<Setting>();
            SubCategorys  = new HashSet<SubCategory>();
        }
        [Required , StringLength(100) ]
        public string FitsrName { get; set; }

        [Required, StringLength(100)]
        public string lastName { get; set; }
        [Required]
        public DateTime CreatDate { get; set; }

        [Required , StringLength(100)]
        public string Gander { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Range(1,90)]
        public int Age { get; set; }
        public DateTime? lastlogin { get; set; }
        public double? Discount { get; set; }
        public bool? ISActive { get; set; }

        [StringLength(50)]
        public string? language { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Prodact> Prodacts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Governorate> Governorates { get; set; }
        public virtual ICollection<Category> Categorys { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Slider> Sliders { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<SubCategory> SubCategorys { get; set; }

    }
}
