using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Reports
{
    public class GetOrderReportHandler
    : IRequestHandler<GetOrderReportQuery, byte[]>
    {
        private readonly WarehouseDbContext _context;

        public GetOrderReportHandler(WarehouseDbContext context)
        {
            _context = context;
        }

       
        /// Generates PDF file with full order details.
      
        public async Task<byte[]> Handle(
            GetOrderReportQuery request,
            CancellationToken cancellationToken)
        {
            // استعلام Orders مع الـ OrderItems و Products
            var orders = request.Id.HasValue
                ? await _context.Orders
                    .Where(o => o.Id == request.Id.Value && o.IsDeleted==false)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync(cancellationToken)
                : await _context.Orders
                    .Where(o => o.IsDeleted==false)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync(cancellationToken);

            // إنشاء التقرير PDF
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header()
                        .Text("Orders Report")
                        .Bold().FontSize(20).AlignCenter();

                    page.Content().Column(col =>
                    {
                        foreach (var order in orders)
                        {
                            col.Item().Text($"Order #{order.Id} - {order.OrderNumber}").Bold();
                            col.Item().Text($"Status: {order.Status} | Total: {order.TotalAmount:C} | Created: {order.CreatedAt}");
                            col.Item().LineHorizontal(1);

                            col.Item().Text("Items:").Underline();

                            foreach (var item in order.OrderItems)
                            {
                                col.Item().Text(
                                    $"{item.Product.Name} | Quantity: {item.Quantity} | UnitPrice: {item.UnitPrice:C} | Subtotal: {(item.Quantity * item.UnitPrice):C}");
                            }

                            col.Item().LineHorizontal(2);
                        }
                    });

                });
            }).GeneratePdf();
        }
    }
}
