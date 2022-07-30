using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
namespace ECommerce.BLL.DTO
{
    public class NotificationDto
    {
        public int ID { get; set; }

        [StringLength(450) , Required]
        public string UserID { get; set; }

        [StringLength(100), Required]
        public string Title { get; set; }

        [StringLength(100), Required]
        public string Subject { get; set; }

        [StringLength(255), Required]
        public string Messege { get; set; }
        public string Icon { get; set; }


    }
}
