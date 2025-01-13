using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.ViewModels;
using System.Linq.Expressions;

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

            // 金額篩選
            if (parameters.Amount.HasValue && parameters.Amount.Value > 0)
            {
                query = query.Where(o => o.TotalAmount > parameters.Amount.Value);
            }

            if (!string.IsNullOrEmpty(parameters.AmountSort))
            {
                if (parameters.AmountSort.ToLower() == "asc")
                {
                    query = query.OrderBy(o => o.TotalAmount);
                }
                else if (parameters.AmountSort.ToLower() == "desc")
                {
                    query = query.OrderByDescending(o => o.TotalAmount);
                }
            }
            else
            {
                // 默認排序: 按日期排序
                query = query.OrderBy(o => o.OrderDate);
            }

            return query;
        }
        /// <summary>
        /// 取得訂單總筆數
        /// </summary>
        /// <returns></returns>
        public int GettotalRecords(OrderQueryParameters parameters)
        { 
            return GetOrder(parameters).Count();
        }

        public int GetTotalPrice(int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Year == year)
                .Sum(o => o.TotalAmount);
        }
        public int GetTotalQuantity(int year)
        {
            return _context.OrderDetails
                .Where(od => od.Order.OrderDate.Year == year)
                .Sum(od => od.ProductId);
        }
        public IEnumerable<OrderIndexVm> GetMonthlySalesData(int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => (o.OrderDate.Month-1)/3)// 每3個月分組，0: Q1, 1: Q2, 2: Q3, 3: Q4
                .Select(g => new OrderIndexVm
                {
                    Quarter = g.Key+1,
                    TotalAmount = g.Sum(o => o.TotalAmount),
                    TotalQuantity = g.Sum(o => o.OrderDetails.Count())
                })
                .OrderBy(qsd=>qsd.Quarter).ToList();

        }



    }


}
