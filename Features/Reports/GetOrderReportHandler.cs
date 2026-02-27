using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Warehouse.Common;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Features.Reports
{
    public class GetOrderReportHandler
       : IRequestHandler<GetOrderReportQuery, Result<byte[]>>
    {
        private readonly WarehouseDbContext _context;

        public GetOrderReportHandler(WarehouseDbContext context) => _context = context;

        public async Task<Result<byte[]>> Handle(
            GetOrderReportQuery request,
            CancellationToken cancellationToken)
        {
            // 1. جيب البيانات
            var orders = request.Id.HasValue
                ? await _context.Orders
                    .Where(o => o.Id == request.Id.Value && !o.IsDeleted)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync(cancellationToken)
                : await _context.Orders
                    .Where(o => !o.IsDeleted)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .ToListAsync(cancellationToken);

            // 2. لو Id اتبعت وملقاش حاجة
            if (request.Id.HasValue && !orders.Any())
                return Result<byte[]>.Failure(
                    $"Order with Id {request.Id} not found or has been deleted.");

            // 3. لو مفيش orders خالص
            if (!orders.Any())
                return Result<byte[]>.Failure("No orders found.");

            // 4. Generate PDF
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Header
                    page.Header().PaddingBottom(10).BorderBottom(1)
                        .BorderColor(Colors.Grey.Medium)
                        .Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Warehouse System")
                                   .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                                col.Item().Text("Orders Report")
                                   .FontSize(13).FontColor(Colors.Grey.Darken2);
                                col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                                   .FontSize(9).FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(100).AlignRight().Column(col =>
                            {
                                col.Item().Text($"Total Orders: {orders.Count}")
                                   .FontSize(11).Bold().FontColor(Colors.Blue.Darken2);
                                col.Item().Text($"Total Amount: {orders.Sum(o => o.TotalAmount ?? 0):C}")
                                   .FontSize(11).Bold().FontColor(Colors.Green.Darken2);
                            });
                        });

                    // Content
                    page.Content().PaddingTop(15).Column(col =>
                    {
                        foreach (var order in orders)
                        {
                            // Order Header
                            col.Item().PaddingTop(10)
                                .Background(Colors.Blue.Darken2)
                                .Padding(8)
                                .Row(row =>
                                {
                                    row.RelativeItem().Text(
                                        $"Order #{order.Id} - {order.OrderNumber}")
                                        .Bold().FontColor(Colors.White).FontSize(12);

                                    row.ConstantItem(120).AlignRight().Text(
                                        $"Status: {order.Status}")
                                        .FontColor(Colors.White).FontSize(10);
                                });

                            // Order Info
                            col.Item().Background(Colors.Grey.Lighten3).Padding(6)
                                .Row(row =>
                                {
                                    row.RelativeItem().Text(
                                        $"Created: {order.CreatedAt:yyyy-MM-dd HH:mm}")
                                        .FontSize(10).FontColor(Colors.Grey.Darken2);

                                    row.ConstantItem(150).AlignRight().Text(
                                        $"Total: {order.TotalAmount:C}")
                                        .FontSize(10).Bold().FontColor(Colors.Green.Darken2);
                                });

                            // Items Table
                            col.Item().PaddingTop(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);  // Product
                                    columns.RelativeColumn(1);  // Quantity
                                    columns.RelativeColumn(1);  // UnitPrice
                                    columns.RelativeColumn(1);  // Subtotal
                                });

                                // Table Header
                                table.Header(header =>
                                {
                                    void HeaderCell(string text) =>
                                        header.Cell()
                                              .Background(Colors.Grey.Darken1)
                                              .Padding(5).AlignCenter()
                                              .Text(text).FontColor(Colors.White)
                                              .Bold().FontSize(10);

                                    HeaderCell("Product");
                                    HeaderCell("Quantity");
                                    HeaderCell("Unit Price");
                                    HeaderCell("Subtotal");
                                });

                                // Table Rows
                                foreach (var (item, index) in order.OrderItems.Select((x, i) => (x, i)))
                                {
                                    var bgColor = index % 2 == 0
                                        ? Colors.White
                                        : Colors.Grey.Lighten4;

                                    table.Cell().Background(bgColor).Padding(5)
                                         .Text(item.Product?.Name ?? "N/A").FontSize(10);

                                    table.Cell().Background(bgColor).Padding(5)
                                         .AlignCenter().Text(item.Quantity.ToString()).FontSize(10);

                                    table.Cell().Background(bgColor).Padding(5)
                                         .AlignRight().Text($"{item.UnitPrice:C}").FontSize(10);

                                    table.Cell().Background(bgColor).Padding(5)
                                         .AlignRight()
                                         .Text($"{item.Quantity * item.UnitPrice:C}")
                                         .FontSize(10).Bold();
                                }
                            });

                            col.Item().PaddingTop(5).LineHorizontal(1)
                               .LineColor(Colors.Grey.Lighten1);
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(" of ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                });
            }).GeneratePdf();

            return Result<byte[]>.Ok(pdf, "Report generated successfully.");
        }
    }
}
