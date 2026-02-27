using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Stocks.Create
{
    public class CreateStockHandler
        : IRequestHandler<CreateStockCommand, Result<StockResponse>>
    {
        private readonly WarehouseDbContext _context;

        public CreateStockHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<StockResponse>> Handle(
            CreateStockCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Product موجود؟
            var productExists = await _context.Products
                .AnyAsync(p => p.Id == request.ProductId, cancellationToken);
            if (!productExists)
                return Result<StockResponse>.Failure(
                    $"Product with Id {request.ProductId} not found.");

            // 2. Warehouse موجود؟
            var warehouseExists = await _context.Warehouses
                .AnyAsync(w => w.Id == request.WarehouseId, cancellationToken);
            if (!warehouseExists)
                return Result<StockResponse>.Failure(
                    $"Warehouse with Id {request.WarehouseId} not found.");

            // 3. Stock موجود بالفعل لنفس الـ Product والـ Warehouse؟
            var stockExists = await _context.Stocks
                .AnyAsync(s =>
                    s.ProductId == request.ProductId &&
                    s.WarehouseId == request.WarehouseId,
                    cancellationToken);
            if (stockExists)
                return Result<StockResponse>.Failure(
                    "Stock already exists for this product in this warehouse.");

            var stock = new Stock
            {
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                Quantity = request.Quantity
            };

            _context.Stocks.Add(stock);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<StockResponse>.Failure(
                    "A conflict occurred while saving. Please try again.");
            }

            return Result<StockResponse>.Ok(
                new StockResponse(
                    stock.Id,
                    stock.ProductId,
                    stock.WarehouseId,
                    stock.Quantity),
                "Stock created successfully.");
        }
    }
}
