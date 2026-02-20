using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Products.Create;
using Warehouse.Features.Products.Delete;
using Warehouse.Features.Products.GetCatalog;
using Warehouse.Features.Products.Update;

namespace Warehouse.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalog()
        {
            var data = await _mediator.Send(new GetCatalogQuery());
            return Ok(data);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched product ID.");
            await _mediator.Send(command);
            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok("Product soft deleted");
        }

    }
}
