using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DAL.Entity
{
    public class Setting : BaseEntity
    {
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Logo { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(100)]
        public string MainColor { get; set; }

        [StringLength(200)]
        public string FaceBook { get; set; }

        [StringLength(200)]
        public string Instagram { get; set; }

        [StringLength(200)]
        public string Youtube { get; set; }

        [StringLength(200)]
        public string Whatsapp { get; set; }

        [StringLength(200)]
        public string TikTok { get; set; }

        [EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string BookingMessage { get; set; }
    }
}
