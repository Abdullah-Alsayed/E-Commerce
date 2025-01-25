using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Stocks.Dtos;
using ECommerce.BLL.Features.Stocks.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository
{
    public class StockRepository : BaseRepository<Stock>, IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
            : base(context) => _context = context;

        public async Task<int> AddStockAsync(Stock stock, CreateStockRequest request)
        {
            var modifyRows = 0;
            _context.Stocks.Add(stock);
            modifyRows++;
            modifyRows += await AddSizeItemAsync(request.Size, request.ProductID);
            modifyRows += await AddColorItemAsync(request.Color, request.ProductID);
            return modifyRows;
        }

        public async Task<int> ReturnProductToStock(List<ProductsOrderRequest> productsOrders)
        {
            var modifyRows = 0;
            var products = productsOrders.Select(x => x.ProductID);
            var stocks = await _context
                .Stocks.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            var productColors = await _context
                .ProductColors.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            var productSizes = await _context
                .ProductSizes.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            foreach (var product in productsOrders)
            {
                stocks.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity +=
                    product.Quantity;
                productColors.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity +=
                    product.Quantity;
                productSizes.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity +=
                    product.Quantity;
                modifyRows += 3;
            }
            return modifyRows;
        }

        public async Task<int> RemoveProductFromStock(List<ProductsOrderRequest> productsOrders)
        {
            var modifyRows = 0;
            var products = productsOrders.Select(x => x.ProductID);
            var stocks = await _context
                .Stocks.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            var productColors = await _context
                .ProductColors.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            var productSizes = await _context
                .ProductSizes.Where(x => products.Contains(x.ProductID))
                .ToListAsync();

            foreach (var product in productsOrders)
            {
                stocks.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity -=
                    product.Quantity;
                productColors.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity -=
                    product.Quantity;
                productSizes.FirstOrDefault(x => x.ProductID == product.ProductID).Quantity -=
                    product.Quantity;
                modifyRows += 3;
            }
            return modifyRows;
        }

        public async Task<int> ReturnItemAsync(ReturnStockRequest request)
        {
            var modifyRows = 0;
            modifyRows += await ReturnSizeItemAsync(request.Size, request.ProductID);
            modifyRows += await ReturnColorItemAsync(request.Color, request.ProductID);
            return modifyRows;
        }

        #region helpers
        private async Task<int> AddSizeItemAsync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rowsAffected = 0;
            var existingProductSizes = await _context
                .ProductSizes.Where(x => x.ProductID == ProductID)
                .ToListAsync();

            foreach (var stockItem in stocks)
            {
                var existingProductSize = existingProductSizes.FirstOrDefault(x =>
                    x.SizeID == stockItem.ID
                );
                if (existingProductSize != null)
                    existingProductSize.Quantity += stockItem.Quantity;
                else
                    await _context.ProductSizes.AddAsync(
                        new ProductSize
                        {
                            ProductID = ProductID,
                            SizeID = stockItem.ID,
                            Quantity = stockItem.Quantity
                        }
                    );
                rowsAffected++;
            }
            return rowsAffected;
        }

        private async Task<int> AddColorItemAsync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rowsAffected = 0;
            var existingProductColors = await _context
                .ProductColors.Where(x => x.ProductID == ProductID)
                .ToListAsync();

            foreach (var stockItem in stocks)
            {
                var existingProductColor = existingProductColors.FirstOrDefault(x =>
                    x.ColorID == stockItem.ID
                );
                if (existingProductColor != null)
                    existingProductColor.Quantity += stockItem.Quantity;
                else
                    await _context.ProductColors.AddAsync(
                        new ProductColor
                        {
                            ProductID = ProductID,
                            ColorID = stockItem.ID,
                            Quantity = stockItem.Quantity
                        }
                    );
                rowsAffected++;
            }
            return rowsAffected;
        }

        private async Task<int> ReturnSizeItemAsync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rowsAffected = 0;
            var existProductSizes = await _context
                .ProductSizes.Where(x => x.ProductID == ProductID)
                .ToListAsync();

            foreach (var Item in stocks)
            {
                var existStock = existProductSizes.FirstOrDefault(x => x.SizeID == Item.ID);
                if (existStock != null)
                {
                    if (existStock.Quantity >= Item.Quantity)
                        existStock.Quantity -= Item.Quantity;
                    else
                        existStock.Quantity = 0;

                    rowsAffected++;
                }
            }
            return rowsAffected;
        }

        private async Task<int> ReturnColorItemAsync(List<StockItemDto> stocks, Guid ProductID)
        {
            var rowsAffected = 0;
            var existProductColors = await _context
                .ProductColors.Where(x => x.ProductID == ProductID)
                .ToListAsync();

            foreach (var Item in stocks)
            {
                var existStock = existProductColors.FirstOrDefault(x => x.ColorID == Item.ID);
                if (existStock != null)
                {
                    if (existStock.Quantity >= Item.Quantity)
                        existStock.Quantity -= Item.Quantity;
                    else
                        existStock.Quantity = 0;

                    rowsAffected++;
                }
            }
            return rowsAffected;
        }
        #endregion
    }
}
