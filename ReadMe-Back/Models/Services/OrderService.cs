using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Infra;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Services
{
    public class OrderService
    {
        private readonly OrderEFRepo _orderEFRepo;

        public OrderService(OrderEFRepo orderEFRepo)
        {
            _orderEFRepo = orderEFRepo;
        }

        public List<OrderVm> GetOrder(OrderQueryParameters parameters)
        {
            try
            {
                // 從 Repository 獲取查詢結果
                var query = _orderEFRepo.GetOrder(parameters);

                // 將查詢轉換為 List 並返回
                return query.ToList();
            }
            catch (Exception ex)
            {
                // 記錄日誌或處理錯誤
                Console.WriteLine($"Error in Service Layer: {ex.Message}");
                throw; // 將例外重新拋出
            }
        }
        public Paged<OrderVm> GetPageOrder(OrderQueryParameters parameters)
        {
            // 獲取篩選後的訂單數據
            var query = _orderEFRepo.GetOrder(parameters);

            // 獲取分頁數據
            var totalRecords =_orderEFRepo.GettotalRecords(parameters);

            //分頁處裡
            var data = query.Skip((parameters.PageNumber-1)*parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
            // 構造 Pagination 對象
            var pagination = new Pagination(
                parameters.PageNumber,
                parameters.PageSize,
                totalRecords,
                parameters.Keyword);
            return new Paged<OrderVm>(data, pagination, parameters.Keyword, null);
        }

    }
}
