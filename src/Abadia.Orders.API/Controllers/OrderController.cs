using Abadia.Orders.Application.Contracts;
using Abadia.Orders.Application.Queries.Interfaces;
using Abadia.Orders.Application.Queries.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abadia.Orders.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderQuery _orderQuery;

        public OrderController(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        //TODO ADD FEATURE GATE
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("order")]
        [ProducesResponseType(typeof(OrdersDetailsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OrdersDetailsViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrders(string contractId)
        {
            var result = await _orderQuery.GetOrdersByContractId(contractId);

            if (!result.Any())
            {
                return StatusCode(404, result);
            }

            return StatusCode(200, result);
        }
    }
}
