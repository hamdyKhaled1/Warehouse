using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Orders.CreateOrder;
using Warehouse.Features.Orders.Delete;
using Warehouse.Features.Orders.GetOrder.GetAll;
using Warehouse.Features.Orders.GetOrder.GetById;
using Warehouse.Features.Orders.Update;

namespace Warehouse.Features.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

     
        /// Creates new order.
       
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
      
        /// Retrieves all  orders
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }


        /// Gets order by Id.
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched order ID.");
            await _mediator.Send(command);
            return Ok("Order updated successfully.");
        }

       
        ///  delete order.
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteOrderCommand(id));
            return Ok();
        }
    }
}
