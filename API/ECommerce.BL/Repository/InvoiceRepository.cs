using System;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Invoices.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Repository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository { }

public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
        : base(context) => _context = context;

    public async Task<Invoice> GetInvoiceProductsAsync(ReturnInvoiceRequest request)
    {
        try
        {
            var invoice = await _context
                .Invoices.Include(x => x.Order)
                .ThenInclude(x => x.ProductOrders)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == request.ID);
            return invoice;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.InnerException.Message);
            return new Invoice();
        }
    }
}
