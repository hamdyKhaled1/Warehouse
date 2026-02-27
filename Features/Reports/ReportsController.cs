using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Features.Reports
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersReport([FromQuery] int? id)
        {
            var result = await _mediator.Send(new GetOrderReportQuery(id));

            if (!result.Success)
                return NotFound(new { result.Success, result.Message });

            return File(
                result.Data!,
                "application/pdf",
                id.HasValue ? $"Order_{id}.pdf" : "AllOrdersReport.pdf");
        }
    }
}


