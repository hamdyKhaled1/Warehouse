using MediatR;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common;
using Warehouse.Features.OrderItems;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Orders.CreateOrder
{
    public class CreateOrderHandler
        : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
    {
        private readonly WarehouseDbContext _context;

        public CreateOrderHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<OrderResponse>> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            // 1. توليد رقم طلب تلقائي فريد ومحمي
            // التنسيق: ORD-تاريخ اليوم-جزء عشوائي
            string generatedOrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";

            // 2. حساب إجمالي المبلغ برمجياً لضمان الأمان 100%
            decimal calculatedTotal = request.Items.Sum(i => i.Quantity * i.UnitPrice);

           

            // 3. بناء كائن الطلب مع العناصر (Mapping)
            var order = new Order
            {
                OrderNumber = generatedOrderNumber,
                TotalAmount = calculatedTotal, // القيمة المحسوبة
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

           
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<OrderResponse>.Ok(
     new OrderResponse(
         order.Id,
         order.OrderNumber,
         order.TotalAmount,
         order.Status,
         order.CreatedAt,
        
         order.OrderItems.Select(i => new OrderItemResponse(
             i.Id,
             i.OrderId,
             i.ProductId,
             i.Quantity,
             i.UnitPrice)).ToList()
     ),
     "Order Created Successfully.");
        }
    }
}
