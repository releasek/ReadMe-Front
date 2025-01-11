using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReadMe_Back.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderApiController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetOrder([FromQuery] OrderQueryParameters parameters)
        {
            var order = _orderService.GetOrder(parameters);
            return Ok(order);
        }
    }
}
