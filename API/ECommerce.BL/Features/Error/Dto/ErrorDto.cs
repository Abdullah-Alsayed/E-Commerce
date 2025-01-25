using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;

namespace ECommerce.BLL.Features.Error.Dto
{
    public class ErrorDto
    {
        public DateTime Date { get; set; }
        public List<ErrorLogDto> Errors { get; set; }
        public int Count { get; set; }
    }
}
