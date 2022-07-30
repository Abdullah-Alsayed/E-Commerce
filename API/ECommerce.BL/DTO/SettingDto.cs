using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.DAL
{
    public class SettingDto
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(255)]
        public string? Logo { get; set; }

        [StringLength(150)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? MainColor { get; set; }

        [StringLength(200)]
        public string? FaceBook { get; set; }

        [StringLength(200)]
        public string? Instagram { get; set; }

        [StringLength(200)]
        public string? Youtube { get; set; }

        [StringLength(200)]
        public string? whatsapp { get; set; }

        [EmailAddress,StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Paner1 { get; set; }

        [StringLength(255)]
        public string? Paner2 { get; set; }

        [StringLength(255)]
        public string? Paner3 { get; set; }

        [StringLength(255)]
        public string? Paner4 { get; set; }

        [StringLength(255)]
        public string? Paner5 { get; set; }


        [StringLength(450)]
        public string? EditBy { get; set; }
        public DateTime? EditAt { get; set; }


    }
}
