using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Orders.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
            : base(context) => _context = context;

        public override async Task<List<Order>> GetAllAsync(
            BaseGridRequest request,
            List<string> Includes = null
        )
        {
            try
            {
                var result = new List<Order>();
                var query = _context
                    .Orders.Include(x => x.Area)
                    .Include(x => x.Status)
                    .Include(x => x.Governorate)
                    .Include(x => x.Voucher)
                    .Include(x => x.User)
                    .Include(x => x.ProductOrders)
                    .ThenInclude(x => x.Product)
                    .AsQueryable();

                query = ApplyDynamicQuery(request, query);
                var total = await query.CountAsync();
                if (total > 0)
                {
                    query = ApplyPagination(request, query);
                    result = await query.AsNoTracking().ToListAsync();
                }
                return result;
            }
            catch (Exception)
            {
                return new List<Order>();
            }
        }

        public async Task<int> AddAsync(Order order, List<ProductOrderRequest> products)
        {
            var modifyRows = 0;
            try
            {
                var governorate = await _context.Governorates.FirstOrDefaultAsync(x =>
                    x.Id == order.GovernorateID
                );
                var productIds = products.Select(x => x.ProductID).ToList();
                var productPrice = await _context
                    .Products.Where(x => productIds.Contains(x.Id))
                    .Select(p => p.Price)
                    .SumAsync();

                var voucherValue = order.VoucherID.HasValue
                    ? await _context
                        .Vouchers.Where(x => x.Id == order.VoucherID.Value)
                        .Select(x => x.Value)
                        .FirstOrDefaultAsync()
                    : 0;

                var orderId = Guid.NewGuid();
                order.Id = orderId;
                order.CreateAt = DateTime.UtcNow;
                order.Count = products.Count;
                order.Tax = governorate?.Tax ?? 0;
                order.SubTotal = (productPrice + order.Tax) - (voucherValue + order.Discount);
                await _context.Orders.AddAsync(order);
                modifyRows++;
                if (products.Any())
                {
                    var productOrder = products
                        .Select(product => new ProductOrder
                        {
                            ID = Guid.NewGuid(),
                            OrderID = orderId,
                            ProductID = product.ProductID,
                            Quantity = product.Quantity
                        })
                        .ToList();
                    await _context.ProductOrders.AddRangeAsync(productOrder);
                    modifyRows += products.Count;
                }
                return modifyRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return modifyRows;
            }
        }
    }
}
