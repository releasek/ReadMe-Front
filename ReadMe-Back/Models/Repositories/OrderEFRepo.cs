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
                                .Sum(n=>n.Quantity)
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

        // 獲取指定年份的總銷售金額
        public int GetTotalPrice(int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Year == year)
                .Sum(o => o.TotalAmount);
        }

        // 獲取指定年份的總銷售數量
        public int GetTotalQuantity(int year)
        {
            return _context.OrderDetails
                .Where(od => od.Order.OrderDate.Year == year)
                .Sum(od => od.Quantity); 
        }

        // 獲取指定年份的季度銷售數據
        public IEnumerable<OrderIndexVm> GetQuarterlySalesData(int year)
        {
            return _context.OrderDetails
                .Where(od => od.Order.OrderDate.Year == year)
                .GroupBy(od => new
                {
                    Year = od.Order.OrderDate.Year,
                    Quarter = (od.Order.OrderDate.Month - 1) / 3 + 1
                })
                .Select(g => new OrderIndexVm
                {
                    Year = g.Key.Year,
                    Quarter = g.Key.Quarter,
                    TotalQuantity = g.Sum(od => od.Quantity) // 匯總數量
                })
                .OrderBy(q => q.Year)
                .ThenBy(q => q.Quarter)
                .ToList();
        }

        // 獲取指定年份的每季度銷售金額與數量
        public IEnumerable<OrderIndexVm> GetMonthlySalesData(int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => (o.OrderDate.Month - 1) / 3)
                .Select(g => new OrderIndexVm
                {
                    Quarter = g.Key + 1,
                    TotalAmount = g.Sum(o => o.TotalAmount),
                    TotalQuantity = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity))
                })
                .OrderBy(q => q.Quarter)
                .ToList();
        }



    }


}
