using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetById
{
    public class GetOrderByIdHandler
        : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
    {
        private readonly WarehouseDbContext _context;

        public GetOrderByIdHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<OrderResponse>> Handle(
            GetOrderByIdQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _context.Orders
    .Include(o => o.OrderItems) // تأكيد تحميل الأصناف من الداتابيز
    .Where(o => o.Id == request.Id)
    .Select(o => new OrderResponse(
        o.Id,
        o.OrderNumber,
        o.TotalAmount,
        o.Status, 
        o.CreatedAt,
        
        o.OrderItems.Select(i => new OrderItemResponse(
            i.Id,
            i.OrderId,
            i.ProductId,
            i.Quantity,
            i.UnitPrice)).ToList()
    ))
    .FirstOrDefaultAsync(cancellationToken);

            if (order is null)
                return Result<OrderResponse>.Failure(
                    $"Order with Id {request.Id} not found.");

            return Result<OrderResponse>.Ok(order);
        }
    }
}
