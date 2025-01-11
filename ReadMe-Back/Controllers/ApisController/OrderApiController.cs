using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ReadMe_Back.Models.Services;
using ReadMe_Back.Models.ViewModels;

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
        [HttpGet("GetOrder")]
        public IActionResult GetOrder([FromQuery] OrderQueryParameters parameters)
        {
            try
            {
                var orders= _orderService.GetOrder(parameters);
                return Ok(orders);
            }
            catch(Exception ex) 
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

    }
}
