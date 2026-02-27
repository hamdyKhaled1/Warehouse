using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Stocks.Create;
using Warehouse.Features.Stocks.Delete;
using Warehouse.Features.Stocks.GetAll;
using Warehouse.Features.Stocks.GetById;
using Warehouse.Features.Stocks.Update;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStocksQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetStockByIdQuery(id));
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> Create([FromBody] CreateStockCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Success = false, Message = "Id in URL must match Id in body." });

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteStockCommand(id));
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}