using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Features.Orders.GetOrder.DTO;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.GetOrder.GetById
{
    public class GetOrderByIdHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly WarehouseDbContext _context;

        public GetOrderByIdHandler(WarehouseDbContext context)
        {
            _context = context;
        }

       
        /// Retrieves  order by Id 
        
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Where(o => o.Id == request.Id && o.IsDeleted==false) // Soft Delete filter
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Order with Id {request.Id} not found or deleted.");

            return order;
        }
    }
}
