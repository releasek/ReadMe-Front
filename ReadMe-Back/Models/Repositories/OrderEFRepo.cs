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




    }


}
