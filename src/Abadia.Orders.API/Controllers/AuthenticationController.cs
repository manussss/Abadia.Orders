using Abadia.Orders.Application.Commands.Authentication;
using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Domain.AuthenticationModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abadia.Orders.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync(UserModel user)
        {
            var result = await _mediator.Send(new CreateUserCommand(user.UserName, user.Email, user.Password, user.IsAdmin));

            if (!result.Valid)
                return StatusCode((int)result.ResponseCode, result.Message);

            return StatusCode((int)result.ResponseCode);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync(LoginModel login)
        {
            var result = await _mediator.Send(new LoginCommand(login.UserName, login.Password));

            if (!result.Valid)
                return StatusCode((int)result.ResponseCode, result.Message);

            return StatusCode((int)result.ResponseCode, result.Data);
        }
    }
}
