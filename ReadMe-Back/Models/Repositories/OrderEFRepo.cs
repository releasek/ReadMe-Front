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
                            // 計算每個 OrderId 的產品數量總和
                            TotalQuantity = _context.OrderDetails
                            .Where(od => od.OrderId == o.Id) // 過濾條件
                            .Count(),
                        };
            if (parameters.StartDate != null)
            {
                query = query.Where(o => o.OrderDate >= parameters.StartDate);
            }
            if (parameters.EndDate != null)
            {
                query = query.Where(o => o.OrderDate <= parameters.EndDate);
            }
            if (parameters.Quantity.HasValue)
            {
                query = query.Where(o => o.TotalQuantity > parameters.Quantity.Value);
            }

            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(o=>
                o.OrderName.Contains(parameters.Keyword) ||
                o.UserName.Contains(parameters.Keyword));

            }

            if (!string.IsNullOrEmpty(parameters.QuantitySort))
            {
                if (parameters.QuantitySort.ToLower() == "asc")
                    query = query.OrderBy(o => o.TotalQuantity);
                else if (parameters.QuantitySort.ToLower() == "desc")
                    query = query.OrderByDescending(o => o.TotalQuantity);
            }

            if (!string.IsNullOrEmpty(parameters.AmountSort))
            {
                if (parameters.AmountSort.ToLower() == "asc")
                    query = query.OrderBy(o => o.TotalAmount);
                else if (parameters.AmountSort.ToLower() == "desc")
                    query = query.OrderByDescending(o => o.TotalAmount);
            }
            return query;

        }

    }

}
