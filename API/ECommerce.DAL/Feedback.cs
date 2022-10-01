using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL
{
    public class Feedback
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Comment { get; set; }

        [ForeignKey("User"), StringLength(450), Required]
        public string UserID { get; set; }
        [Required]
        public int Rating { get; set; }

        public virtual User User { get; set; }
    }
}
