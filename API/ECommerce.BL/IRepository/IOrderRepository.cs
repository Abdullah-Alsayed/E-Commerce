using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Governorates.Requests;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;

namespace ECommerce.BLL.IRepository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<int> AddAsync(Order order, List<ProductOrderRequest> products);
    }
}
