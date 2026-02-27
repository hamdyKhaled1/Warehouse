using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.OrderItems.Create;
using Warehouse.Features.OrderItems.Delete;
using Warehouse.Features.OrderItems.Get;
//using Warehouse.Features.OrderItems.Update;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderItemsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _mediator.Send(
                new GetOrderItemsByOrderIdQuery(orderId));
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,WarehouseStaff")]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Manager")]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemCommand command)
        //{
        //    if (id != command.Id)
        //        return BadRequest(new { Success = false, Message = "Id in URL must match Id in body." });

        //    var result = await _mediator.Send(command);
        //    return result.Success ? Ok(result) : BadRequest(result);
        //}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteOrderItemCommand(id));
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}