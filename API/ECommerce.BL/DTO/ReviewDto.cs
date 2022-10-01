using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace ECommerce.BLL.DTO
{
    public class ReviewDto
    {
        public int ID { get; set; }

        [Required]
        public int ProdactID { get; set; }

        [Required , StringLength(255)]
        public string review { get; set; }

        [Required , Range(1,10)]
        public int Rate { get; set; }

    }


}