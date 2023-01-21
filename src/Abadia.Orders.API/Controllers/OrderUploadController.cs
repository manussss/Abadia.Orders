using Abadia.Orders.Application.Commands.Upload;
using Abadia.Orders.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abadia.Orders.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public class OrderUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //TODO ADD FEATURE GATE
    [HttpPost("upload/xls")]
    [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseContract), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadOrder(IFormFile file)
    {
        var result = await _mediator.Send(new UploadXlsCommand(file));

        return StatusCode(result.ResponseCode.GetHashCode(), result);
    }
}
