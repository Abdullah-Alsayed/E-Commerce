using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class ProdactRepository : BaseRepository<Prodact>, IProdactRepository
    {

        public ProdactRepository(Applicationdbcontext context , IHostingEnvironment hosting) : base(context , hosting)
        {
        }

        public async Task<Prodact> AddToChart(int ID)
        {
          var Prodact = await FindAsync(ID);
            return Prodact;
        }
    }
}
