using ReadMe_Back.Models.EFModels;
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

    }
}
