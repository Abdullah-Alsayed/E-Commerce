using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using ECommerce.DAL;

namespace ECommerce.BLL.DTO
{
    public class ExpenseDto
    {
        public int ID { get; set; }

        [Required]
        public double Amount { get; set; }

        [StringLength(150),Required]
        public string Reference { get; set; }
    }
}
