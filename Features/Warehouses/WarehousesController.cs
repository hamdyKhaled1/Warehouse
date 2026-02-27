using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Warehouses.Create;
using Warehouse.Features.Warehouses.Delete;
using Warehouse.Features.Warehouses.GetAll;
using Warehouse.Features.Warehouses.GetById;
using Warehouse.Features.Warehouses.Update;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllWarehousesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetWarehouseByIdQuery(id));
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarehouseCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Success = false, Message = "Id in URL must match Id in body." });

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWarehouseCommand(id));
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}