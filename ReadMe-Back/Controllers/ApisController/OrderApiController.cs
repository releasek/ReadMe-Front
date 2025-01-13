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
                   (!parameters.Amount.HasValue || parameters.Amount <= 0) &&
                    string.IsNullOrWhiteSpace(parameters.Keyword) &&
                    string.IsNullOrWhiteSpace(parameters.AmountSort)))
                {
                    return Ok(new
                    {
                        message = "沒有符合條件的訂單。",
                        data = new List<OrderVm>(), // 回傳空列表
                        pagination = new
                        {
                            pageNumber = 0,
                            pageSize = 0,
                            totalRecords = 0,
                            totalPages = 0
                        }
                    });
                }

                // 調用服務層獲取分頁數據
                var pagedOrders = _orderService.GetPageOrder(parameters);

                // 如果分頁數據為空，回傳空結果（防止資料錯誤）
                if (pagedOrders == null || pagedOrders.Data == null || !pagedOrders.Data.Any())
                {
                    return Ok(new
                    {
                        message = "查詢結果為空。",
                        data = new List<OrderVm>(),
                        pagination = new
                        {
                            pageNumber = parameters.PageNumber,
                            pageSize = parameters.PageSize,
                            totalRecords = 0,
                            totalPages = 0
                        }
                    });
                }

                return Ok(new
                {
                    message = "成功獲取訂單資料。",
                    data = pagedOrders.Data,
                    pagination = new
                    {
                        pageNumber = pagedOrders.Pagination.PageNumber,
                        pageSize = pagedOrders.Pagination.PageSize,
                        totalRecords = pagedOrders.Pagination.TotalRecords,
                        totalPages = pagedOrders.Pagination.TotalPages
                    }
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
