using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Features.Account.Login;
using Warehouse.Features.Account.Register;

namespace Warehouse.Features.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register/staff")]
        public async Task<IActionResult> RegisterStaff(
            [FromBody] RegisterStaffCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register/manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterManager(
            [FromBody] RegisterManagerCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
    }
}
