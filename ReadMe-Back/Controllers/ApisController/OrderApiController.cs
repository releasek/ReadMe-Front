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
                    var data = _orderService.GetAllPageOrder(parameters);
                    return Ok(new
                    {
                        message = "成功獲取訂單資料。",
                        data = data.Data,
                        pagination = new
                        {
                            pageNumber = data.Pagination.PageNumber,
                            pageSize = data.Pagination.PageSize,
                            totalRecords = data.Pagination.TotalRecords,
                            totalPages = data.Pagination.TotalPages
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

        /// <summary>
        /// 獲取年度總銷售金額
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>總銷售金額</returns>
        [HttpGet("totalAmount")]
        public IActionResult GetTotalAmount(int year)
        {
            var totalAmount = _orderService.GetTotalAmount(year);
            return Ok(totalAmount);
        }

        /// <summary>
        /// 獲取年度總銷售數量
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>總銷售數量</returns>
        [HttpGet("totalQuantity")]
        public IActionResult GetTotalQuantity(int year)
        {
            var totalQuantity = _orderService.GetTotalQuantity(year);
            return Ok(totalQuantity);
        }

        /// <summary>
        /// 獲取每季的銷售數據
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>每季銷售數據</returns>
        [HttpGet("quarterlyData")]
        public IActionResult GetQuarterlySalesData(int year)
        {
            var salesData = _orderService.GetMonthlySalesData(year);
            return Ok(new { Year = year, QuarterlySalesData = salesData });
        }
    }
}
