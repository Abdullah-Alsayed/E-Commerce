using ECommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.DTO
{
    public class HomeDto
    {
        public Setting Setting { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<SubCategory> SubCategories { get; set; }
        public IEnumerable<Prodact> Prodacts { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<ProdactImg> ProdactImgs { get; set; }

    }
}
