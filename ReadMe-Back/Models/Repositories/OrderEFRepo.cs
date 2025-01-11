using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Models.Repositories
{
    public class OrderEFRepo
    {
        private readonly AppDbContext _context;

        public OrderEFRepo(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<OrderVm> GetOrder(OrderQueryParameters parameters)
        {
            var query = from o in _context.Orders
                        join u in _context.Users on o.UserId equals u.Id
                        select new OrderVm
                        {
                            OrderId = o.Id,
                            OrderName = o.OrderName,
                            TotalAmount = o.TotalAmount,
                            OrderDate = o.OrderDate,
                            UserName = u.Name,
                            TotalQuantity = _context.OrderDetails
                                .Where(od => od.OrderId == o.Id)
                                .Count(),
                        };

            // 日期篩選
            if (parameters.StartDate != null)
            {
                query = query.Where(o => o.OrderDate >= parameters.StartDate);
            }

            if (parameters.EndDate != null)
            {
                query = query.Where(o => o.OrderDate <= parameters.EndDate);
            }

            // 關鍵字過濾
            if (!string.IsNullOrWhiteSpace(parameters.Keyword))
            {
                query = query.Where(o =>
                    o.OrderName.Contains(parameters.Keyword) ||
                    o.UserName.Contains(parameters.Keyword));
            }

            // 排序條件
            if (!string.IsNullOrEmpty(parameters.QuantitySort))
            {
                query = parameters.QuantitySort.ToLower() == "asc"
                    ? query.OrderBy(o => o.TotalQuantity)
                    : query.OrderByDescending(o => o.TotalQuantity);
            }
            else if (!string.IsNullOrEmpty(parameters.AmountSort))
            {
                query = parameters.AmountSort.ToLower() == "asc"
                    ? query.OrderBy(o => o.TotalAmount)
                    : query.OrderByDescending(o => o.TotalAmount);
            }
            else
            {
                query = query.OrderBy(o => o.OrderDate); // 默認按日期排序
            }

            return query;
        }


    }

}
