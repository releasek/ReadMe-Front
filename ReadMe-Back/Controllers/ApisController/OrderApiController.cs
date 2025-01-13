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
                // 檢查是否有有效的篩選條件
                if (parameters == null ||
                    (parameters.StartDate == null &&
                    parameters.EndDate == null &&
                    string.IsNullOrWhiteSpace(parameters.Keyword) &&
                    string.IsNullOrWhiteSpace(parameters.AmountSort)))
                {
                    return Ok(new
                    {
                        message = "沒有符合條件的訂單。",
                        data = new List<OrderVm>() // 回傳空列表，保持格式一致
                    });
                }

                // 如果有參數，調用服務層進行查詢
                var orders = _orderService.GetOrder(parameters);

                return Ok(new
                {
                    message = "成功獲取訂單資料。",
                    data = orders
                });
            }
            catch (Exception ex)
            {
                // 捕獲例外並返回 500 錯誤
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
