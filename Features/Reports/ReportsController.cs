using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Features.Reports
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        /// Generates PDF report for Orders.
        /// If id is provided → returns single order report.
        /// If null → returns all orders report.
      
       
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersReport([FromQuery] int? id)
        {
            try
            {
                // استدعاء MediatR Handler لتوليد PDF
                var pdfBytes = await _mediator.Send(new GetOrderReportQuery(id));

                // إرجاع ملف PDF للمستخدم
                return File(
                    pdfBytes,
                    "application/pdf",
                    id.HasValue ? $"Order_{id}.pdf" : "AllOrdersReport.pdf"
                );
            }
            catch (Exception ex)
            {
                // معالجة أي خطأ أثناء توليد التقرير
                return StatusCode(500, $"Error generating PDF report: {ex.Message}");
            }
        }
    }
}


