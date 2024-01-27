using System.ComponentModel.DataAnnotations;
using System;

namespace ECommerce.BLL.DTO
{
    public class ColorDto
    {
        public Guid ID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }
    }
}
