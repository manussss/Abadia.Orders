using Abadia.Orders.Application.Commands.Upload;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abadia.Orders.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //TODO ADD FEATURE GATE
    //TODO ADD PRODUCERESPONSETYPE
    [HttpPost]
    public async Task<IActionResult> UploadOrder(IFormFile file)
    {
        var result = await _mediator.Send(new UploadXlsCommand(file));

        return StatusCode(result.ResponseCode.GetHashCode(), result);
    }
}
